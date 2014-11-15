namespace King.Service
{
    using King.Service.Data;
    using King.Service.Timing;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Scheduled Manager
    /// </summary>
    public abstract class CoordinatedTask : RecurringTask
    {
        #region Members
        /// <summary>
        /// Task Core
        /// </summary>
        protected readonly ICoordinator taskCore;
        #endregion

        #region Constructors
        /// <summary>
        /// Scheduled Manager Constructor
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="periodInSeconds">Period In Seconds</param>
        protected CoordinatedTask(string connectionString, double periodInSeconds = BaseTimes.MaximumStorageTiming)
            : this(new Coordinator(TimeSpan.FromSeconds(periodInSeconds), connectionString))
        {
        }

        /// <summary>
        /// Constructor for Mocking
        /// </summary>
        /// <param name="coordinator">Coordinator</param>
        public CoordinatedTask(ICoordinator coordinator)
            : base(BaseTimes.MinimumStorageTiming, coordinator.PeriodInSeconds + 1)
        {
            this.taskCore = coordinator;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize Table Task
        /// </summary>
        public virtual IRunnable InitializeTask()
        {
            return this.taskCore.InitializeTask();
        }

        /// <summary>
        /// Run
        /// </summary>
        /// <param name="state">State</param>
        public override void Run(object state)
        {
            this.RunAsync().Wait();
        }

        /// <summary>
        /// Run Async
        /// </summary>
        /// <returns>Task</returns>
        public async Task RunAsync()
        {
            var timing = Stopwatch.StartNew();
            var type = this.GetType();
            var startTime = DateTime.UtcNow;
            var successful = false;

            Trace.TraceInformation("{0}: Task Checking.", base.ServiceName);

            try
            {
                var ready = await taskCore.Check(type);
                if (ready)
                {
                    Trace.TraceInformation("{0}: Task Starting.", base.ServiceName);

                    var identifier = Guid.NewGuid();

                    await this.taskCore.Start(type, identifier, startTime);

                    try
                    {
                        this.Run();
                        successful = true;
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("{0}: {1}", base.ServiceName, ex.ToString());
                        successful = false;
                    }

                    await this.taskCore.Complete(type, identifier, startTime, DateTime.UtcNow, successful);
                }
                else
                {
                    Trace.TraceInformation("{0}: Task not Started.", base.ServiceName);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0}: {1}", base.ServiceName, ex.ToString());
                successful = false;
            }
            finally
            {
                timing.Stop();
            }

            Trace.TraceInformation("{0}: Coordinated Task Completed (Duration: {1}).", base.ServiceName, timing.Elapsed);
        }
        #endregion
    }
}
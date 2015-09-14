namespace King.Service
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using King.Service.Data;
    using King.Service.Timing;

    /// <summary>
    /// Coordinated Task
    /// </summary>
    public abstract class CoordinatedTask : RecurringTask
    {
        #region Members
        /// <summary>
        /// Coordinator
        /// </summary>
        protected readonly ICoordinator coordinator;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
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
            : base(coordinator.PeriodInSeconds + 1)
        {
            this.coordinator = coordinator;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize Task (Table Storage)
        /// </summary>
        public virtual IRunnable InitializeTask()
        {
            return this.coordinator.InitializeTask();
        }

        /// <summary>
        /// Run
        /// </summary>
        public override void Run()
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

            Trace.TraceInformation("{0}: Task Checking.", base.Name);

            try
            {
                var ready = await coordinator.Check(type);
                if (ready)
                {
                    Trace.TraceInformation("{0}: Task Starting.", base.Name);

                    var identifier = Guid.NewGuid();

                    await this.coordinator.Start(type, identifier, startTime);

                    try
                    {
                        this.Run();
                        successful = true;
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("{0}: {1}", base.Name, ex.ToString());
                        successful = false;
                    }

                    await this.coordinator.Complete(type, identifier, startTime, DateTime.UtcNow, successful);
                }
                else
                {
                    Trace.TraceInformation("{0}: Task not Started.", base.Name);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0}: {1}", base.Name, ex.ToString());
                successful = false;
            }
            finally
            {
                timing.Stop();
            }

            Trace.TraceInformation("{0}: Coordinated Task Completed (Duration: {1}).", base.Name, timing.Elapsed);
        }
        #endregion
    }
}
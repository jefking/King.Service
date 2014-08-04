namespace King.Service
{
    using King.Service.Data;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Scheduled Manager
    /// </summary>
    public abstract class CoordinatedTask : TaskManager
    {
        #region Members
        /// <summary>
        /// Task Core
        /// </summary>
        private readonly ICoordinator taskCore;
        #endregion

        #region Constructors
        /// <summary>
        /// Scheduled Manager Constructor
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="periodInSeconds">Period In Seconds</param>
        protected CoordinatedTask(string connectionString, double periodInSeconds = 60)
            : this(new Coordinator(TimeSpan.FromSeconds(periodInSeconds), connectionString))
        {
        }

        /// <summary>
        /// Constructor for Mocking
        /// </summary>
        /// <param name="coordinator">Coordinator</param>
        public CoordinatedTask(ICoordinator coordinator)
            : base(15, coordinator.PeriodInSeconds + 1)
        {
            this.taskCore = coordinator;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize Table Task
        /// </summary>
        public IRunnable InitializeTask()
        {
            return this.taskCore.InitializeTask();
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="state">State</param>
        public override void Run(object state)
        {
            var timing = Stopwatch.StartNew();
            var type = this.GetType();
            var serviceName = type.ToString();
            var startTime = DateTime.UtcNow;
            var successful = false;

            Trace.TraceInformation("{0}: Task Checking.", serviceName);

            try
            {
                if (taskCore.Check(type))
                {
                    Trace.TraceInformation("{0}: Task Starting.", serviceName);

                    var identifier = Guid.NewGuid();

                    var task = this.taskCore.Start(type, identifier, startTime);
                    task.Wait();

                    try
                    {
                        this.Run();
                        successful = true;
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("{0}: {1}", serviceName, ex.Message);
                        successful = false;
                    }

                    task = this.taskCore.Complete(type, identifier, startTime, DateTime.UtcNow, successful);
                    task.Wait();
                }
                else
                {
                    Trace.TraceInformation("{0}: Task not Started.", serviceName);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0}: {1}", serviceName, ex.Message);
                successful = false;
            }
            finally
            {
                timing.Stop();
            }

            Trace.TraceInformation("{0}: Coordinated Task Completed (Duration: {1}).", this.GetType().ToString(), timing.Elapsed);
        }
        #endregion
    }
}
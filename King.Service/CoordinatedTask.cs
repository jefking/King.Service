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
        protected readonly ICoordinator taskCore;
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
        public virtual IRunnable InitializeTask()
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
            var startTime = DateTime.UtcNow;
            var successful = false;

            Trace.TraceInformation("{0}: Task Checking.", base.serviceName);

            try
            {
                var ready = taskCore.Check(type).Result;
                if (ready)
                {
                    Trace.TraceInformation("{0}: Task Starting.", base.serviceName);

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
                        Trace.TraceError("{0}: {1}", base.serviceName, ex.ToString());
                        successful = false;
                    }

                    task = this.taskCore.Complete(type, identifier, startTime, DateTime.UtcNow, successful);
                    task.Wait();
                }
                else
                {
                    Trace.TraceInformation("{0}: Task not Started.", base.serviceName);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0}: {1}", base.serviceName, ex.ToString());
                successful = false;
            }
            finally
            {
                timing.Stop();
            }

            Trace.TraceInformation("{0}: Coordinated Task Completed (Duration: {1}).", base.serviceName, timing.Elapsed);
        }
        #endregion
    }
}
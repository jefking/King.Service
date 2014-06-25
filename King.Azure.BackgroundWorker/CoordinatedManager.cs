namespace King.Azure.BackgroundWorker
{
    using King.Azure.BackgroundWorker.Data;
    using King.Azure.BackgroundWorker.Data.Model;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Scheduled Manager
    /// </summary>
    public abstract class CoordinatedManager : Manager
    {
        #region Members
        /// <summary>
        /// Task Core
        /// </summary>
        private readonly Coordinator taskCore;
        #endregion

        #region Constructors
        /// <summary>
        /// Scheduled Manager Constructor
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="periodInSeconds">Period In Seconds</param>
        protected CoordinatedManager(string connectionString, double periodInSeconds = 60)
            : base(60, periodInSeconds)
        {
            this.taskCore = new Coordinator(TimeSpan.FromSeconds(periodInSeconds), connectionString);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize Table Task
        /// </summary>
        public Manager InitializeTask()
        {
            return this.taskCore.InitializeTask();
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="state">State</param>
        public override async void Run(object state)
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

                    await this.taskCore.Start(type, identifier, startTime);

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

                    await this.taskCore.Complete(type, identifier, startTime, DateTime.UtcNow, successful);
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

            Trace.TraceInformation("{0}: Task Completed (Duration: {1}).", this.GetType().ToString(), timing.Elapsed);
        }
        #endregion
    }
}
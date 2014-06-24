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
            var entry = new ScheduledTaskEntry(this.GetType())
            {
                StartTime = DateTime.UtcNow,
            };

            Trace.TraceInformation("{0}: Task Checking.", entry.ServiceName);

            try
            {
                if (taskCore.CheckForTask(entry))
                {
                    Trace.TraceInformation("{0}: Task Starting.", entry.ServiceName);

                    entry.Identifier = Guid.NewGuid();
                    entry.CompletionTime = null;

                    await taskCore.InsertOrReplace(entry);

                    try
                    {
                        this.Run();
                        entry.Successful = true;
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("{0}: {1}", entry.ServiceName, ex.Message);
                        entry.Successful = false;
                    }
                    finally
                    {
                        entry.CompletionTime = DateTime.UtcNow;
                    }

                    await taskCore.InsertOrReplace(entry);
                }
                else
                {
                    Trace.TraceInformation("{0}: Task not Started.", entry.ServiceName);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0}: {1}", entry.ServiceName, ex.Message);
                entry.Successful = false;
            }
            finally
            {
                if (!entry.CompletionTime.HasValue)
                {
                    entry.CompletionTime = DateTime.UtcNow;
                }
            }
        }
        #endregion
    }
}
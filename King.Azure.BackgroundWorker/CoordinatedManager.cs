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
        /// Execute
        /// </summary>
        /// <param name="state">State</param>
        public override async void Run(object state)
        {
            var startTime = DateTime.UtcNow;
            var completionTime = DateTime.UtcNow;

            var entry = new ScheduledTaskEntry(this.GetType());

            Trace.WriteLine(string.Format("{0} [{1}] Task Checking.", startTime, entry.ServiceName));

            try
            {
                if (taskCore.CheckForTask(entry))
                {
                    Trace.WriteLine(string.Format("{0} [{1}] Task Started.", DateTime.UtcNow, entry.ServiceName));

                    // Insert a new backup entry to table
                    entry.Identifier = Guid.NewGuid();
                    entry.StartTime = DateTime.UtcNow;

                    await taskCore.InsertOrReplace(entry);

                    try
                    {
                        this.Run();
                        entry.Successful = true;
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError(string.Format("{0}:{1}", this.GetType().ToString(), ex.Message));
                        entry.Successful = false;
                    }
                    finally
                    {
                        entry.CompletionTime = DateTime.UtcNow;
                    }

                    // Update entry in table
                    await taskCore.InsertOrReplace(entry);
                }
                else
                {
                    Trace.WriteLine(string.Format("{0} [{1}] No Action Required.", DateTime.UtcNow, entry.ServiceName));
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("{0}:{1}", this.GetType().ToString(), ex.Message));
                entry.Successful = false;
            }
            finally
            {
                completionTime = DateTime.UtcNow;
            }

            Trace.WriteLine(string.Format("{0} [{1}] Task Completed. Success: {2}", DateTime.UtcNow, entry.ServiceName, entry.Successful));
        }
        #endregion
    }
}
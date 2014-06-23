namespace King.Azure.BackgroundWorker.Data
{
    using King.Azure.BackgroundWorker.Data.Model;
    using System;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Scheduled Task Core
    /// </summary>
    public class Coordinator : TableStorage, ICoordinator
    {
        #region Members
        /// <summary>
        /// Table Name
        /// </summary>
        public const string TableName = "schedulingtasks";

        /// <summary>
        /// Period of Timer
        /// </summary>
        private readonly TimeSpan period;

        /// <summary>
        /// Maximum Duration before Retry
        /// </summary>
        private readonly TimeSpan retryInterval = TimeSpan.FromMinutes(5);
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="period">Period</param>
        /// <param name="connectionString">Connection String</param>
        public Coordinator(TimeSpan period, string connectionString)
            : base(TableName, connectionString)
        {
            if (TimeSpan.Zero >= period)
            {
                throw new ArgumentException("period");
            }
            
            this.period = period;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize Table Task
        /// </summary>
        public Manager InitializeTask()
        {
            return new InitializeTableTask(this);
        }

        /// <summary>
        /// Determine whether a new task needs to be executed
        /// </summary>
        /// <param name="entry">Scheduled Task Entry</param>
        /// <returns>True if need to execute, false if not</returns>
        public bool CheckForTask(ScheduledTaskEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            var performTask = true;

            Trace.TraceInformation(string.Format("{0}: Querying scheduled tasks table for the latest task.", entry.ServiceName));

            // Peek the table first to determine if there's any task to execute
            // Query the table by partition key (type, year, month)
            var records = base.QueryByPartition<ScheduledTaskEntry>(entry.PartitionKey);

            if (records != null && records.Count() > 0)
            {
                var latest = records.OrderByDescending(x => x.StartTime).First();

                Trace.TraceInformation(string.Format("{0}: Latest task found in table: Partition: {2} Id: {3} StartTime: {4} CompletionTime: {5}", entry.ServiceName, latest.PartitionKey, latest.Identifier, latest.StartTime, latest.CompletionTime));

                // 1. If the latest task has been completed, then perform task if
                // - the latest task has been completed more than <period> ago, or
                // - the latest task was unsuccessful
                // 2. If the latest task has been started but not completed yet,
                // then perform the task if it has been started more than <backupRetryInterval> ago
                performTask = (latest.CompletionTime.HasValue) ?
                    DateTime.UtcNow.Subtract(latest.CompletionTime.Value) >= period || !latest.Successful :
                    DateTime.UtcNow.Subtract(latest.StartTime) >= retryInterval;
            }

            return performTask;
        }
        #endregion
    }
}
namespace King.Azure.BackgroundWorker.Data
{
    using King.Azure.BackgroundWorker.Data.Model;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Scheduled Task Core
    /// </summary>
    public class Coordinator : ICoordinator
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

        /// <summary>
        /// Table Storage
        /// </summary>
        private readonly ITableStorage storage = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="period">Period</param>
        /// <param name="connectionString">Connection String</param>
        public Coordinator(TimeSpan period, string connectionString)
            : this(new TableStorage(TableName, connectionString), period)
        {
        }

        public Coordinator(ITableStorage storage, TimeSpan period)
        {
            if (null == storage)
            {
                throw new ArgumentNullException("storage");
            }
            if (TimeSpan.Zero >= period)
            {
                throw new ArgumentException("period");
            }

            this.period = period;
            this.storage = storage;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize Table Task
        /// </summary>
        public Manager InitializeTask()
        {
            return new InitializeTableTask(this.storage);
        }

        /// <summary>
        /// Determine whether a new task needs to be executed
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>True if need to execute, false if not</returns>
        public bool Check(Type type)
        {
            if (null == type)
            {
                throw new ArgumentNullException("type");
            }

            var entry = new ScheduledTaskEntry(type);

            var performTask = true;

            Trace.TraceInformation(string.Format("{0}: Querying scheduled tasks table for the latest task.", entry.ServiceName));

            // Peek the table first to determine if there's any task to execute
            // Query the table by partition key (type, year, month)
            var records = this.storage.QueryByPartition<ScheduledTaskEntry>(entry.PartitionKey);

            if (records != null && records.Count() > 0)
            {
                var latest = records.OrderByDescending(x => x.StartTime).First();

                Trace.TraceInformation("{0}: Latest task found in table: Partition: {1} Id: {2} StartTime: {3} CompletionTime: {4}", entry.ServiceName, latest.PartitionKey, latest.Identifier, latest.StartTime, latest.CompletionTime);

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

        public async Task Start(Type type, Guid identifier, DateTime start)
        {
            if (null == type)
            {
                throw new ArgumentNullException("type");
            }
            if (Guid.Empty == identifier)
            {
                throw new ArgumentException("identifier");
            }

            var entry = new ScheduledTaskEntry(type)
            {
                Identifier = identifier,
                StartTime = start,
            };

            await this.storage.InsertOrReplace(entry);
        }

        public async Task Complete(Type type, Guid identifier, DateTime start, DateTime end, bool success)
        {
            if (null == type)
            {
                throw new ArgumentNullException("type");
            }
            if (Guid.Empty == identifier)
            {
                throw new ArgumentException("identifier");
            }

            var entry = new ScheduledTaskEntry(type)
            {
                Identifier = identifier,
                StartTime = start,
                CompletionTime = end,
                Successful = success,
            };

            await this.storage.InsertOrReplace(entry);
        }
        #endregion
    }
}
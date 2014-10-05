namespace King.Service.Data
{
    using King.Azure.Data;
    using King.Service.Data.Model;
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
        protected readonly TimeSpan period;

        /// <summary>
        /// Maximum Duration before Retry
        /// </summary>
        protected readonly TimeSpan retryInterval;

        /// <summary>
        /// Table Storage
        /// </summary>
        protected readonly ITableStorage storage = null;
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

        /// <summary>
        /// Constructor for Mocking
        /// </summary>
        /// <param name="storage">Storage</param>
        /// <param name="period">Period</param>
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
            this.retryInterval = TimeSpan.FromSeconds(period.TotalSeconds * 2.5);
            this.storage = storage;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize Table Task
        /// </summary>
        public virtual TaskManager InitializeTask()
        {
            return new InitializeStorageTask(this.storage);
        }

        /// <summary>
        /// Determine whether a new task needs to be executed
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>True if need to execute, false if not</returns>
        public virtual async Task<bool> Check(Type type)
        {
            if (null == type)
            {
                throw new ArgumentNullException("type");
            }

            var entry = new ScheduledTaskEntry(type);

            var performTask = true;

            Trace.TraceInformation(string.Format("{0}: Querying scheduled tasks for the latest.", entry.ServiceName));

            var records = await this.storage.QueryByPartition<ScheduledTaskEntry>(entry.PartitionKey);

            if (records != null && records.Count() > 0)
            {
                var latest = records.OrderByDescending(x => x.StartTime).First();

                Trace.TraceInformation("{0}: Latest task found: StartTime: {1} CompletionTime: {2}", entry.ServiceName, latest.StartTime, latest.CompletionTime);

                performTask = (latest.CompletionTime.HasValue) ?
                    DateTime.UtcNow.Subtract(latest.CompletionTime.Value) >= period || !latest.Successful :
                    DateTime.UtcNow.Subtract(latest.StartTime) >= retryInterval;
            }

            return performTask;
        }

        /// <summary>
        /// Start
        /// </summary>
        /// <param name="type">Task Type</param>
        /// <param name="identifier">Identifier</param>
        /// <param name="start">Start</param>
        /// <returns>Task</returns>
        public virtual async Task Start(Type type, Guid identifier, DateTime start)
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

        /// <summary>
        /// Complete
        /// </summary>
        /// <param name="type">Task Type</param>
        /// <param name="identifier">Identifier</param>
        /// <param name="start">Start</param>
        /// <param name="end">End</param>
        /// <param name="success">Success</param>
        /// <returns>Task</returns>
        public virtual async Task Complete(Type type, Guid identifier, DateTime start, DateTime end, bool success)
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

        #region Properties
        /// <summary>
        /// Period In Seconds
        /// </summary>
        public virtual int PeriodInSeconds
        {
            get
            {
                return (int)this.period.TotalSeconds;
            }
        }
        #endregion
    }
}
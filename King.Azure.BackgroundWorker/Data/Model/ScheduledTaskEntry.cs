namespace King.Azure.BackgroundWorker.Data.Model
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;

    /// <summary>
    /// Scheduled Task Entry
    /// </summary>
    public class ScheduledTaskEntry : TableEntity
    {
        #region Constructor
        /// <summary>
        /// Default Constructpor
        /// </summary>
        public ScheduledTaskEntry()
        {
        }
        
        public ScheduledTaskEntry(Type service)
            :this(service.ToString())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceName">Service Name</param>
        public ScheduledTaskEntry(string serviceName)
        {
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                throw new ArgumentException("serviceName");
            }

            this.ServiceName = serviceName;
            this.PartitionKey = ScheduledTaskEntry.GenerateLogsPartitionKey(serviceName);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets and sets Identifier
        /// </summary>
        public Guid? Identifier
        {
            get
            {
                if (string.IsNullOrEmpty(this.RowKey))
                {
                    return null;
                }
                else
                {
                    return Guid.Parse(this.RowKey);
                }
            }
            set
            {
                if (value == null)
                {
                    this.RowKey = null;
                }
                else
                {
                    this.RowKey = value.ToString();
                }
            }
        }

        /// <summary>
        /// Gets and sets Start Time
        /// </summary>
        public DateTime StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets Completion Time
        /// </summary>
        public DateTime? CompletionTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the Service Name
        /// </summary>
        public string ServiceName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets Successful
        /// </summary>
        public bool Successful
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generate the partition key
        /// Format: {type}-{year}-{month}
        /// </summary>
        /// <param name="type">Backup data type</param>
        /// <returns>Partition key</returns>
        public static string GenerateLogsPartitionKey(string serviceName)
        {
            return string.Format("{0}-{1:yyyy}-{1:MM}", serviceName, DateTime.UtcNow);
        }
        #endregion
    }
}
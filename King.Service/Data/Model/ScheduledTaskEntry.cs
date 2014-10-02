namespace King.Service.Data.Model
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
        
        /// <summary>
        /// Service Type
        /// </summary>
        /// <param name="service">Type</param>
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
        /// Identifier
        /// </summary>
        public virtual Guid? Identifier
        {
            get
            {
                return string.IsNullOrEmpty(this.RowKey) ? (Guid?)null : Guid.Parse(this.RowKey);
            }
            set
            {
                this.RowKey = null == value ? null : value.ToString();
            }
        }

        /// <summary>
        /// Start Time
        /// </summary>
        public virtual DateTime StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// Completion Time
        /// </summary>
        public virtual DateTime? CompletionTime
        {
            get;
            set;
        }

        /// <summary>
        /// the Service Name
        /// </summary>
        public virtual string ServiceName
        {
            get;
            set;
        }

        /// <summary>
        /// Successful
        /// </summary>
        public virtual bool Successful
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
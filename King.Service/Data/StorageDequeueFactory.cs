namespace King.Service.Data
{
    using System;
    using System.Collections.Generic;
    using King.Azure.Data;
    using King.Service.Data.Model;
    using King.Service.Scalability;

    /// <summary>
    /// Storage Dequeue Factory
    /// </summary>
    public class StorageDequeueFactory
    {
        #region Members
        /// <summary>
        /// Connection String
        /// </summary>
        protected readonly string connectionString = null;

        /// <summary>
        /// Throughput metrics
        /// </summary>
        protected readonly IQueueThroughput throughput = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        public StorageDequeueFactory(string connectionString)
            : this(connectionString, new QueueThroughput())
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="throughput">Queue Throughput</param>
        public StorageDequeueFactory(string connectionString, IQueueThroughput throughput = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("connectionString");
            }

            this.connectionString = connectionString;
            this.throughput = throughput ?? new QueueThroughput();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates the Queue, and Loads Dynamic Dequeuer
        /// </summary>
        /// <typeparam name="T">Passthrough</typeparam>
        /// <param name="setup">Setup</param>
        /// <param name="processor">Processor</param>
        /// <returns>Tasks</returns>
        public virtual IEnumerable<IRunnable> Tasks<T>(IQueueSetup<T> setup)
        {
            if (null == setup)
            {
                throw new ArgumentNullException("setup");
            }
            
            yield return new InitializeStorageTask(new StorageQueue(setup.Name, this.connectionString));
            yield return this.Dequeue<T>(setup);
        }

        /// <summary>
        /// Dequeue Task (Storage Queue Auto Scaler)
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="setup">Setup</param>
        /// <returns>Storage Queue Auto Scaler</returns>
        public virtual IRunnable Dequeue<T>(IQueueSetup<T> setup)
        {
            if (null == setup)
            {
                throw new ArgumentNullException("setup");
            }

            var messagesPerScaleUnit = this.throughput.MessagesPerScaleUnit(setup.Priority);
            var minimum = this.throughput.MinimumScale(setup.Priority);
            var maximum = this.throughput.MaximumScale(setup.Priority);
            var checkScaleInMinutes = this.throughput.CheckScaleEvery(setup.Priority);
            var connection = new QueueConnection<T>()
            {
                ConnectionString = this.connectionString,
                Setup = setup,
            };
            var queue = new StorageQueue(connection.Setup.Name, connection.ConnectionString);

            return new StorageQueueAutoScaler<T>(queue, connection, this.throughput, messagesPerScaleUnit, minimum, maximum, checkScaleInMinutes);
        }
        #endregion
    }
}
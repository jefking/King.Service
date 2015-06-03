namespace King.Service.Data
{
    using King.Azure.Data;
    using King.Service.Scalability;
    using System;
    using System.Collections.Generic;
    using King.Service.Data.Model;


    /// <summary>
    /// Storage Dequeue Factory
    /// </summary>
    /// <typeparam name="T">Processor Type</typeparam>
    public class StorageDequeueFactory<T> : ITaskFactory<IQueueSetup<T>>
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
        /// <param name="throughput">Throughput</param>
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
        /// <param name="setup">Setup</param>
        /// <param name="processor">Processor</param>
        /// <returns>Tasks</returns>
        public virtual IEnumerable<IRunnable> Tasks(IQueueSetup<T> setup)
        {
            if (null == setup)
            {
                throw new ArgumentNullException("setup");
            }

            var queue = new StorageQueue(setup.Name, setup.ConnectionString);
            yield return new InitializeStorageTask(queue);
            yield return this.DequeueTask(queue, setup);
        }

        /// <summary>
        /// Dequeue Task
        /// </summary>
        /// <param name="queue">Queue</param>
        /// <param name="setup">Setup</param>
        /// <returns>Storage Queue Auto Scaler</returns>
        public virtual IRunnable DequeueTask(IStorageQueue queue, IQueueSetup<T> setup)
        {
            if (null == queue)
            {
                throw new ArgumentNullException("queue");
            }
            if (null == setup)
            {
                throw new ArgumentNullException("setup");
            }

            var messagesPerScaleUnit = this.throughput.MessagesPerScaleUnit(setup.Priority);
            var minimum = this.throughput.MinimumScale(setup.Priority);
            var maximum = this.throughput.MaximumScale(setup.Priority);
            var checkScaleInMinutes = this.throughput.CheckScaleEvery(setup.Priority);

            return new StorageQueueAutoScaler<T>(queue, setup, messagesPerScaleUnit, minimum, maximum, checkScaleInMinutes);
        }

        /// <summary>
        /// Create Scalable Task
        /// </summary>
        /// <param name="queueName">Queue Name</param>
        /// <param name="processor">Processor</param>
        /// <param name="priority">Priority</param>
        /// <returns>Runnable</returns>
        public virtual IRunnable Scalable(string queueName, IProcessor<T> processor, QueuePriority priority = QueuePriority.Low)
        {
            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentException("queueName");
            }
            if (null == processor)
            {
                throw new ArgumentNullException("processor");
            }

            var queue = new StorageQueue(queueName, connectionString);
            var creator = new DequeueTaskCreator<T>(queueName, connectionString, processor, priority, this.throughput);
            var messagesPerScaleUnit = this.throughput.MessagesPerScaleUnit(priority);
            var minimumScale = this.throughput.MinimumScale(priority);
            var maximumScale = this.throughput.MaximumScale(priority);
            var checkScaleEvery = this.throughput.CheckScaleEvery(priority);
            return new QueueSimplifiedScaler(queue, creator, messagesPerScaleUnit, minimumScale, maximumScale, checkScaleEvery);
        }
        #endregion
    }
}
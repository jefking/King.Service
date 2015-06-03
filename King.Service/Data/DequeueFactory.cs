namespace King.Service.Data
{
    using System;
    using King.Azure.Data;
    using King.Service.Data.Model;
    using King.Service.Scalability;

    /// <summary>
    /// Dequeue Factory, for auto-scaling dequeue classes
    /// </summary>
    /// <typeparam name="T">Dequeue Model</typeparam>
    public class DequeueFactory<T>
    {
        #region Members
        /// <summary>
        /// Throughput metrics
        /// </summary>
        protected readonly IQueueThroughput throughput = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="throughput">Queue Throughput</param>
        public DequeueFactory(IQueueThroughput throughput = null)
        {
            this.throughput = throughput ?? new QueueThroughput();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create Runnable Task
        /// </summary>
        /// <param name="queueName">Queue Name</param>
        /// <param name="connectionString">Connection String</param>
        /// <param name="processor">Processor</param>
        /// <param name="priority">Priority</param>
        /// <returns>Runnable</returns>
        public virtual IRunnable Runnable(string queueName, string connectionString, IProcessor<T> processor, QueuePriority priority = QueuePriority.Low)
        {
            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentException("queueName");
            }
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("connectionString");
            }
            if (null == processor)
            {
                throw new ArgumentNullException("processor");
            }

            var queue = new StorageQueue(queueName, connectionString);
            var creator = new DequeueTaskCreator<T>(queueName, connectionString, processor, priority);
            var messagesPerScaleUnit = this.throughput.MessagesPerScaleUnit(priority);
            var minimumScale = this.throughput.MinimumScale(priority);
            var maximumScale = this.throughput.MaximumScale(priority);
            var checkScaleEvery = this.throughput.CheckScaleEvery(priority);
            return new QueueSimplifiedScaler(queue, creator, messagesPerScaleUnit, minimumScale, maximumScale, checkScaleEvery);
        }
        #endregion
    }
}
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
    public abstract class DequeueFactory<T>
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
        /// Create Scalable Task
        /// </summary>
        /// <param name="queueName">Queue Name</param>
        /// <param name="processor">Processor</param>
        /// <param name="priority">Priority</param>
        /// <returns>Runnable</returns>
        public abstract IRunnable Scalable(string queueName, IProcessor<T> processor, QueuePriority priority = QueuePriority.Low);
        #endregion
    }
}
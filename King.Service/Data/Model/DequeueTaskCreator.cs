namespace King.Service.Data.Model
{
    using System;
    using King.Azure.Data;
    using King.Service.Scalability;

    /// <summary>
    /// Dequeue Task Creator
    /// </summary>
    /// <typeparam name="T">Dequeue Model</typeparam>
    public class DequeueTaskCreator<T> : ITaskCreator
    {
        #region Members
        /// <summary>
        /// Queue Name
        /// </summary>
        protected readonly string queueName;

        /// <summary>
        /// Connection String
        /// </summary>
        protected readonly string connectionString;

        /// <summary>
        /// Processor
        /// </summary>
        protected readonly IProcessor<T> processor;

        /// <summary>
        /// Priority
        /// </summary>
        protected readonly QueuePriority priority;

        /// <summary>
        /// Throughput metrics
        /// </summary>
        protected readonly IQueueThroughput throughput = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="queueName">Queue Name</param>
        /// <param name="connectionString">Connection String</param>
        /// <param name="processor">Processor</param>
        /// <param name="priority">Priority</param>
        public DequeueTaskCreator(string queueName, string connectionString, IProcessor<T> processor, QueuePriority priority = QueuePriority.Low, IQueueThroughput throughput = null)
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

            this.queueName = queueName;
            this.connectionString = connectionString;
            this.processor = processor;
            this.priority = priority;
            this.throughput = throughput ?? new QueueThroughput();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Scalable Task
        /// </summary>
        public virtual Func<IScalable> Task
        {
            get
            {
                return () => {
                    var dequeue = new StorageDequeueBatchDynamic<T>(this.queueName, this.connectionString, this.processor);
                    return this.throughput.Runner(dequeue, this.priority);
                };
            }
        }
        #endregion
    }
}
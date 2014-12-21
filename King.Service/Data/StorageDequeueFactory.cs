namespace King.Service.Data
{
    using King.Azure.Data;
    using King.Service.Scalability;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Storage Dequeue Factory
    /// </summary>
    /// <typeparam name="T">Processor Type</typeparam>
    public class StorageDequeueFactory<T> : ITaskFactory<IQueueSetup<T>>
    {
        #region Members
        /// <summary>
        /// Queue Throughput
        /// </summary>
        protected readonly IQueueThroughput throughput = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructors
        /// </summary>
        public StorageDequeueFactory()
            :this(new QueueThroughput())
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="throughput">Throughput</param>
        public StorageDequeueFactory(IQueueThroughput throughput)
        {
            if (null == throughput)
            {
                throw new ArgumentNullException("throughput");
            }

            this.throughput = throughput;
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
            byte minimum = this.throughput.MinimumScale(setup.Priority);
            byte maximum = this.throughput.MaximumScale(setup.Priority);
            var checkScaleInMinutes = this.throughput.CheckScaleEvery(setup.Priority);

            return new StorageQueueAutoScaler<T>(queue, setup, messagesPerScaleUnit, minimum, maximum, checkScaleInMinutes);
        }
        #endregion
    }
}
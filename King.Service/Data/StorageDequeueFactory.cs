namespace King.Service.Data
{
    using King.Azure.Data;
    using King.Service.Data.Model;
    using King.Service.Scalability;
    using King.Service.Timing;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Storage Dequeue Factory
    /// </summary>
    /// <typeparam name="T">Processor Type</typeparam>
    public class StorageDequeueFactory<T> : ITaskFactory<IQueueSetup>
    {
        #region Methods
        /// <summary>
        /// Creates the Queue, and Loads Dynamic Dequeuer
        /// </summary>
        /// <param name="setup">Setup</param>
        /// <param name="processor">Processor</param>
        /// <returns>Tasks</returns>
        public virtual IEnumerable<IRunnable> Tasks(IQueueSetup setup)
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
        public virtual IRunnable DequeueTask(IStorageQueue queue, IQueueSetup setup)
        {
            if (null == queue)
            {
                throw new ArgumentNullException("queue");
            }
            if (null == setup)
            {
                throw new ArgumentNullException("setup");
            }

            var messagesPerScaleUnit = QueueScaler<T>.MessagesPerScaleUnitDefault;
            byte minimum = 1;
            byte maximum = 2;
            var checkScaleInMinutes = BaseTimes.ScaleCheck;

            switch (setup.Priority)
            {
                case QueuePriority.Low:
                    messagesPerScaleUnit = 1000;
                    checkScaleInMinutes = 4;
                    break;
                case QueuePriority.Medium:
                    messagesPerScaleUnit = 100;
                    maximum = 5;
                    checkScaleInMinutes = 2;
                    break;
                case QueuePriority.High:
                    minimum = 2;
                    maximum = 10;
                    checkScaleInMinutes = 1;
                    break;
            }

            return new StorageQueueAutoScaler<T>(queue, setup, messagesPerScaleUnit, minimum, maximum, checkScaleInMinutes);
        }
        #endregion
    }
}
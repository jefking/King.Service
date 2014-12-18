namespace King.Service.Data
{
    using King.Azure.Data;
    using King.Service.Data.Model;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Storage Dequeue Factory
    /// </summary>
    /// <typeparam name="T">Processor Type</typeparam>
    public class StorageDequeueFactory<T> : ITaskFactory<QueueSetup>
    {
        #region Methods
        /// <summary>
        /// Tasks; Creates the Queue, and Generates Dequeuer
        /// </summary>
        /// <param name="setup">Setup</param>
        /// <param name="processor">Processor</param>
        /// <returns>Tasks</returns>
        public IEnumerable<IRunnable> Tasks(QueueSetup setup, IProcessor<T> processor)
        {
            if (null == setup)
            {
                throw new ArgumentNullException("setup");
            }
            if (null == processor)
            {
                throw new ArgumentNullException("processor");
            }

            var q = new StorageQueue(setup.Name, setup.ConnectionString);
            yield return new InitializeStorageTask(q);
            yield return new StorageQueueAutoScaler<T>(q, setup);
        }
        #endregion
    }
}
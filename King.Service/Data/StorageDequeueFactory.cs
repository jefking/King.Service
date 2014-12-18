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
        /// Creates the Queue, and Generates Dequeuer
        /// </summary>
        /// <param name="setup">Setup</param>
        /// <param name="processor">Processor</param>
        /// <returns>Tasks</returns>
        public IEnumerable<IRunnable> Tasks(QueueSetup setup)
        {
            if (null == setup)
            {
                throw new ArgumentNullException("setup");
            }

            var q = new StorageQueue(setup.Name, setup.ConnectionString);
            yield return new InitializeStorageTask(q);
            yield return new StorageQueueAutoScaler<T>(q, setup);
        }
        #endregion
    }
}
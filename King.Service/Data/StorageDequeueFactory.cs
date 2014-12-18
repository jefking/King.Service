﻿namespace King.Service.Data
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

            var q = new StorageQueue(setup.Name, setup.ConnectionString);
            yield return new InitializeStorageTask(q);

            var messagesPerScaleUnit = QueueScaler<T>.MessagesPerScaleUnitDefault;
            byte minimum = 1;
            byte maximum = 2;
            var checkScaleInMinutes = BaseTimes.ScaleCheck;

            switch (setup.Priority)
            {
                case QueuePriority.Low:
                    messagesPerScaleUnit = 50;
                    checkScaleInMinutes = 4;
                    break;
                case QueuePriority.Medium:
                    messagesPerScaleUnit = 25;
                    maximum = 5;
                    break;
                case QueuePriority.High:
                    minimum = 2;
                    maximum = 10;
                    checkScaleInMinutes = 1;
                    break;
            }

            yield return new StorageQueueAutoScaler<T>(q, setup, messagesPerScaleUnit, minimum, maximum, checkScaleInMinutes);
        }
        #endregion
    }
}
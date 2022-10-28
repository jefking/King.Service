﻿namespace King.Service.Azure
{
    using Data;
    using global::Azure.Data.Wrappers;
    using King.Service.Timing;

    /// <summary>
    /// Storage Dequeue Batch
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class StorageDequeueBatch<T> : DequeueBatch<T>
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="connectionString">Connection String</param>
        /// <param name="processor">Processor</param>
        /// <param name="batchCount">Batch Count</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public StorageDequeueBatch(string name, string connectionString, IProcessor<T> processor, byte batchCount = 5, int minimumPeriodInSeconds = BaseTimes.DefaultMinimumTiming, int maximumPeriodInSeconds = BaseTimes.DefaultMaximumTiming)
            : this(new StorageQueue(name, connectionString), processor, batchCount, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="queue">Storage Queue</param>
        /// <param name="processor">Processor</param>
        /// <param name="batchCount">Batch Count</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public StorageDequeueBatch(IStorageQueue queue, IProcessor<T> processor, byte batchCount = 5, int minimumPeriodInSeconds = BaseTimes.DefaultMinimumTiming, int maximumPeriodInSeconds = BaseTimes.DefaultMaximumTiming)
            : base(new StorageQueuePoller<T>(queue), processor, batchCount, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}
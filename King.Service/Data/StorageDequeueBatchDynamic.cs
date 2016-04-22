namespace King.Service.Data
{
    using System;
    using King.Azure.Data;
    using King.Service.Timing;

    /// <summary>
    /// Storage Dequeue Batch Dynamic
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class StorageDequeueBatchDynamic<T> : DequeueBatchDynamic<T>
    {
        #region Members
        /// <summary>
        /// Maximum batchsize = 32
        /// </summary>
        public const byte MaxBatchSize = 32;

        /// <summary>
        /// Set the visibilty in the call to get the batches.
        /// </summary>
        public static readonly TimeSpan VisibilityDuration = TimeSpan.FromSeconds(45);
        #endregion

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
        public StorageDequeueBatchDynamic(string name, string connectionString, IProcessor<T> processor, int minimumPeriodInSeconds = BaseTimes.DefaultMinimumTiming, int maximumPeriodInSeconds = BaseTimes.DefaultMaximumTiming)
            : this(new StorageQueue(name, connectionString, VisibilityDuration), processor, new TimingTracker(VisibilityDuration, MaxBatchSize), minimumPeriodInSeconds, maximumPeriodInSeconds)
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
        public StorageDequeueBatchDynamic(IStorageQueue queue, IProcessor<T> processor, ITimingTracker tracker, int minimumPeriodInSeconds = BaseTimes.DefaultMinimumTiming, int maximumPeriodInSeconds = BaseTimes.DefaultMaximumTiming)
            : base(new StorageQueuePoller<T>(queue), processor, tracker, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}
namespace King.Service.Data
{
    using System;
    using System.Collections.Generic;
    using King.Azure.Data;
    using King.Service.Scalability;
    using King.Service.Timing;

    /// <summary>
    /// Storage Queue AutoScaler
    /// </summary>
    /// <typeparam name="T">Processor Type</typeparam>
    public class StorageQueueAutoScaler<T> : QueueAutoScaler<IQueueConnection<T>>
    {
        #region Members
        /// <summary>
        /// Queue Throughput
        /// </summary>
        protected readonly IQueueThroughput throughput = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="count">Count</param>
        /// <param name="messagesPerScaleUnit">Messages Per-Scale Unit</param>
        /// <param name="setup">Setup</param>
        /// <param name="minimum">Minimum Scale</param>
        /// <param name="maximum">Maximmum Scale</param>
        /// <param name="checkScaleInMinutes">Check Scale Every</param>
        public StorageQueueAutoScaler(IQueueCount count, IQueueConnection<T> setup, ushort messagesPerScaleUnit = QueueScaler<T>.MessagesPerScaleUnitDefault, byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = BaseTimes.ScaleCheck)
            : this(count, setup, new QueueThroughput(), messagesPerScaleUnit, minimum, maximum, checkScaleInMinutes)
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="count">Count</param>
        /// <param name="setup">Setup</param>
        /// <param name="throughput">Throughput</param>
        /// <param name="messagesPerScaleUnit">Messages Per-Scale Unit</param>
        /// <param name="setup">Setup</param>
        /// <param name="minimum">Minimum Scale</param>
        /// <param name="maximum">Maximmum Scale</param>
        /// <param name="checkScaleInMinutes">Check Scale Every</param>
        public StorageQueueAutoScaler(IQueueCount count, IQueueConnection<T> setup, IQueueThroughput throughput, ushort messagesPerScaleUnit = QueueScaler<T>.MessagesPerScaleUnitDefault, byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = BaseTimes.ScaleCheck)
            : base(count, messagesPerScaleUnit, setup, minimum, maximum, checkScaleInMinutes)
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
        /// Scale Unit
        /// </summary>
        /// <param name="queue">Queue Setup</param>
        /// <returns>Scalable Task</returns>
        public override IEnumerable<IScalable> ScaleUnit(IQueueConnection<T> queue)
        {
            if (null == queue)
            {
                throw new ArgumentNullException("setup");
            }

            yield return this.throughput.Runner(this.Runs(queue), queue.Queue.Priority);
        }

        /// <summary>
        /// Runs
        /// </summary>
        /// <param name="queue">Queue Setup</param>
        /// <returns>Dynamic Runs</returns>
        public virtual IDynamicRuns Runs(IQueueConnection<T> queue)
        {
            if (null == queue)
            {
                throw new ArgumentNullException("setup");
            }

            var minimumPeriodInSeconds = this.throughput.MinimumFrequency(queue.Queue.Priority);
            var maximumPeriodInSeconds = this.throughput.MaximumFrequency(queue.Queue.Priority);
            return new StorageDequeueBatchDynamic<T>(queue.Queue.Name, queue.ConnectionString, queue.Queue.Processor(), minimumPeriodInSeconds, maximumPeriodInSeconds);
        }
        #endregion
    }
}
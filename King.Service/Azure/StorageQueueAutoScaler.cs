namespace King.Service.Azure
{
    using Data;
    using King.Azure.Data;
    using King.Service.Scalability;
    using King.Service.Timing;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Storage Queue Auto-Scaler
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
        /// <param name="connection">Queue Connection</param>
        /// <param name="minimum">Minimum Scale</param>
        /// <param name="maximum">Maximmum Scale</param>
        /// <param name="checkScaleInMinutes">Check Scale Every</param>
        public StorageQueueAutoScaler(IQueueCount count, IQueueConnection<T> connection, ushort messagesPerScaleUnit = QueueScaler<T>.MessagesPerScaleUnitDefault, byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = BaseTimes.ScaleCheck)
            : this(count, connection, new QueueThroughput(), messagesPerScaleUnit, minimum, maximum, checkScaleInMinutes)
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="count">Count</param>
        /// <param name="connection">Connection</param>
        /// <param name="throughput">Throughput</param>
        /// <param name="messagesPerScaleUnit">Messages Per-Scale Unit</param>
        /// <param name="connection">Setup</param>
        /// <param name="minimum">Minimum Scale</param>
        /// <param name="maximum">Maximmum Scale</param>
        /// <param name="checkScaleInMinutes">Check Scale Every</param>
        public StorageQueueAutoScaler(IQueueCount count, IQueueConnection<T> connection, IQueueThroughput throughput, ushort messagesPerScaleUnit = QueueScaler<T>.MessagesPerScaleUnitDefault, byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = BaseTimes.ScaleCheck)
            : base(count, messagesPerScaleUnit, connection, minimum, maximum, checkScaleInMinutes)
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
                throw new ArgumentNullException("queue");
            }

            yield return this.throughput.Runner(this.Runs(queue), queue.Setup.Priority);
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

            var frequency = this.throughput.Frequency(queue.Setup.Priority);
            return new StorageDequeueBatchDynamic<T>(queue.Setup.Name, queue.ConnectionString, queue.Setup.Processor(), frequency.Minimum, frequency.Maximum);
        }
        #endregion
    }
}
namespace King.Service.Data
{
    using King.Azure.Data;
    using King.Service.Scalability;
    using King.Service.Timing;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Storage Queue AutoScaler
    /// </summary>
    /// <typeparam name="T">Processor Type</typeparam>
    public class StorageQueueAutoScaler<T> : QueueAutoScaler<IQueueSetup<T>>
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
        public StorageQueueAutoScaler(IQueueCount count, IQueueSetup<T> setup, ushort messagesPerScaleUnit = QueueScaler<T>.MessagesPerScaleUnitDefault, byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = BaseTimes.ScaleCheck)
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
        public StorageQueueAutoScaler(IQueueCount count, IQueueSetup<T> setup, IQueueThroughput throughput, ushort messagesPerScaleUnit = QueueScaler<T>.MessagesPerScaleUnitDefault, byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = BaseTimes.ScaleCheck)
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
        /// <param name="setup">Setup</param>
        /// <returns>Scalable Task</returns>
        public override IEnumerable<IScalable> ScaleUnit(IQueueSetup<T> setup)
        {
            if (null == setup)
            {
                throw new ArgumentNullException("setup");
            }

            yield return this.throughput.Runner(this.Runs(setup), setup.Priority);
        }

        /// <summary>
        /// Runs
        /// </summary>
        /// <param name="setup">Setup</param>
        /// <returns>Dynamic Runs</returns>
        public virtual IDynamicRuns Runs(IQueueSetup<T> setup)
        {
            if (null == setup)
            {
                throw new ArgumentNullException("setup");
            }

            var minimumPeriodInSeconds = this.throughput.MinimumFrequency(setup.Priority);
            var maximumPeriodInSeconds = this.throughput.MaximumFrequency(setup.Priority);
            return new StorageDequeueBatchDynamic<T>(setup.Name, setup.ConnectionString, setup.Get(), minimumPeriodInSeconds, maximumPeriodInSeconds);
        }
        #endregion
    }
}
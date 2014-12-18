namespace King.Service.Data
{
    using King.Azure.Data;
    using King.Service.Data.Model;
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
            : base(count, messagesPerScaleUnit, setup, minimum, maximum, checkScaleInMinutes)
        {
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
            var dequeue = this.Runs(setup);
            yield return this.Runner(dequeue, setup.Priority);
        }

        /// <summary>
        /// Runner
        /// </summary>
        /// <param name="runs">Runs</param>
        /// <param name="priority">Priority</param>
        /// <returns>Scalable</returns>
        public virtual IScalable Runner(IDynamicRuns runs, QueuePriority priority)
        {
            switch (priority)
            {
                case QueuePriority.High:
                    return new BackoffRunner(runs, Strategy.Linear);
                case QueuePriority.Medium:
                    return new BackoffRunner(runs, Strategy.Exponential);
                default:
                    return new AdaptiveRunner(runs);
            }
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

            var minimumPeriodInSeconds = BaseTimes.MinimumStorageTiming;
            var maximumPeriodInSeconds = BaseTimes.MaximumStorageTiming;
            switch (setup.Priority)
            {
                case QueuePriority.High:
                    minimumPeriodInSeconds = 1;
                    maximumPeriodInSeconds = BaseTimes.MinimumStorageTiming;
                    break;
                case QueuePriority.Medium:
                    minimumPeriodInSeconds /= 2;
                    maximumPeriodInSeconds /= 2;
                    break;
            }

            return new StorageDequeueBatchDynamic<T>(setup.Name, setup.ConnectionString, setup.Get(), minimumPeriodInSeconds, maximumPeriodInSeconds);
        }
        #endregion
    }
}
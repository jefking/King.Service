namespace King.Service.Scalability
{
    using King.Service.Data;
    using King.Service.Timing;
    using System;

    /// <summary>
    /// Queue Throughput
    /// </summary>
    public class QueueThroughput : IQueueThroughput
    {
        #region Methods
        /// <summary>
        /// Messages per Scale-unit
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Number of messages per scale unit</returns>
        public virtual ushort MessagesPerScaleUnit(QueuePriority priority)
        {
            switch (priority)
            {
                case QueuePriority.High:
                    return QueueScaler<object>.MessagesPerScaleUnitDefault;
                case QueuePriority.Medium:
                    return 100;
                default:
                    return 1000;
            }
        }

        /// <summary>
        /// Minimum Scale
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Minimum Scale</returns>
        public virtual byte MinimumScale(QueuePriority priority)
        {
            switch (priority)
            {
                case QueuePriority.High:
                    return 2;
                default:
                    return 1;
            }
        }

        /// <summary>
        /// Minimum Frequency
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Minimum Frequency</returns>
        public virtual byte MinimumFrequency(QueuePriority priority)
        {
            switch (priority)
            {
                case QueuePriority.High:
                    return 1;
                case QueuePriority.Medium:
                    return BaseTimes.MinimumStorageTiming / 2;
                default:
                    return BaseTimes.MinimumStorageTiming;
            }
        }

        /// <summary>
        /// Maximum Frequency
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Maximum Frequency</returns>
        public virtual byte MaximumFrequency(QueuePriority priority)
        {
            switch (priority)
            {
                case QueuePriority.High:
                    return BaseTimes.MinimumStorageTiming;
                case QueuePriority.Medium:
                    return BaseTimes.MaximumStorageTiming / 2;
                default:
                    return BaseTimes.MaximumStorageTiming;
            }
        }

        /// <summary>
        /// Maximum Scale
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Maximum Scale</returns>
        public virtual byte MaximumScale(QueuePriority priority)
        {
            switch (priority)
            {
                case QueuePriority.High:
                    return 10;
                case QueuePriority.Medium:
                    return 5;
                default:
                    return 2;
            }
        }

        /// <summary>
        /// Check Scale every (in minutes)
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Check Scale every (in minutes)</returns>
        public virtual byte CheckScaleEvery(QueuePriority priority)
        {
            switch (priority)
            {
                case QueuePriority.High:
                    return 1;
                case QueuePriority.Medium:
                    return 2;
                default:
                    return 4;
            }
        }

        /// <summary>
        /// Runner
        /// </summary>
        /// <param name="runs">Runs</param>
        /// <param name="priority">Priority</param>
        /// <returns>Scalable</returns>
        public virtual IScalable Runner(IDynamicRuns runs, QueuePriority priority)
        {
            if (null == runs)
            {
                throw new ArgumentNullException("runs");
            }

            switch (priority)
            {
                case QueuePriority.High:
                    return new BackoffRunner(runs, Strategy.Linear);
                case QueuePriority.Medium:
                    return new BackoffRunner(runs, Strategy.Exponential);
                default:
                    return new AdaptiveRunner(runs, Strategy.Linear);
            }
        }
        #endregion
    }
}
namespace King.Service.Scalability
{
    using System;
    using King.Service.Data;
    using King.Service.Timing;

    /// <summary>
    /// Queue Throughput
    /// </summary>
    public class QueueThroughput : IQueueThroughput
    {
        #region Methods
        /// <summary>
        /// Messages per Scale-unit
        /// </summary>
        /// <remarks>
        /// Targeting ~1 minute per dequeuer to catch up to Queue Count
        /// </remarks>
        /// <param name="priority">Priority</param>
        /// <returns>Number of messages per scale unit</returns>
        public virtual ushort MessagesPerScaleUnit(QueuePriority priority = QueuePriority.Low)
        {
            switch (priority)
            {
                case QueuePriority.High:
                    return 9500;
                case QueuePriority.Medium:
                    return 1400;
                default:
                    return QueueScaler<object>.MessagesPerScaleUnitDefault;
            }
        }

        /// <summary>
        /// Minimum Frequency to check Queue
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Minimum Frequency</returns>
        public virtual byte MinimumFrequency(QueuePriority priority = QueuePriority.Low)
        {
            switch (priority)
            {
                case QueuePriority.High:
                    return 1;
                case QueuePriority.Medium:
                    return 5;
                default:
                    return BaseTimes.MinimumStorageTiming;
            }
        }

        /// <summary>
        /// Maximum Frequency to check Queue
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Maximum Frequency</returns>
        public virtual byte MaximumFrequency(QueuePriority priority = QueuePriority.Low)
        {
            switch (priority)
            {
                case QueuePriority.High:
                    return BaseTimes.MinimumStorageTiming;
                case QueuePriority.Medium:
                    return 60;
                default:
                    return BaseTimes.MaximumStorageTiming;
            }
        }

        /// <summary>
        /// Minimum Scale (threads)
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Minimum Scale</returns>
        public virtual byte MinimumScale(QueuePriority priority = QueuePriority.Low)
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
        /// Maximum Scale (threads)
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Maximum Scale</returns>
        public virtual byte MaximumScale(QueuePriority priority = QueuePriority.Low)
        {
            switch (priority)
            {
                case QueuePriority.High:
                    return 5;
                case QueuePriority.Medium:
                    return 4;
                default:
                    return 2;
            }
        }

        /// <summary>
        /// Check Scale every (in minutes)
        /// </summary>
        /// <remarks>
        /// Should be kept under 5 min, as Auto-Scale servers should be in place
        /// </remarks>
        /// <param name="priority">Priority</param>
        /// <returns>Check Scale every (in minutes)</returns>
        public virtual byte CheckScaleEvery(QueuePriority priority = QueuePriority.Low)
        {
            switch (priority)
            {
                case QueuePriority.High:
                    return 2;
                case QueuePriority.Medium:
                    return 3;
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
        public virtual IScalable Runner(IDynamicRuns runs, QueuePriority priority = QueuePriority.Low)
        {
            if (null == runs)
            {
                throw new ArgumentNullException("runs");
            }

            switch (priority)
            {
                case QueuePriority.High:
                case QueuePriority.Medium:
                    return new BackoffRunner(runs, Strategy.Linear);
                default:
                    return new AdaptiveRunner(runs, Strategy.Exponential);
            }
        }
        #endregion
    }
}
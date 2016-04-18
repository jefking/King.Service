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
        /// Frequency to check Queue
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Frequency Range</returns>
        public virtual Range<byte> Frequency(QueuePriority priority = QueuePriority.Low)
        {
            ///CHECK THIS
            switch (priority)
            {
                case QueuePriority.High:
                    return new Range<byte>(1, 5);
                case QueuePriority.Medium:
                    return new Range<byte>(5, 15);
                default:
                    return new Range<byte>(BaseTimes.MinimumStorageTiming, BaseTimes.MaximumStorageTiming);
            }
        }

        /// <summary>
        /// Scale (threads)
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Scale</returns>
        public virtual Range<byte> Scale(QueuePriority priority = QueuePriority.Low)
        {
            switch (priority)
            {
                case QueuePriority.High:
                    return new Range<byte>(2, 5);
                case QueuePriority.Medium:
                    return new Range<byte>(1, 4);
                default:
                    return new Range<byte>(1, 2);
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
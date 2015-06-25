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
        /// Frequency to check Queue
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Frequency Range</returns>
        public virtual Range<byte> Frequency(QueuePriority priority = QueuePriority.Low)
        {
            var r = new Range<byte>();
            switch (priority)
            {
                case QueuePriority.High:
                    r.Minimum = 1;
                    r.Maximum = BaseTimes.MinimumStorageTiming;
                    break;
                case QueuePriority.Medium:
                    r.Minimum = 5;
                    r.Maximum = 60;
                    break;
                default:
                    r.Minimum = BaseTimes.MinimumStorageTiming;
                    r.Maximum = BaseTimes.MaximumStorageTiming;
                    break;
            }

            return r;
        }

        /// <summary>
        /// Scale (threads)
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Scale</returns>
        public virtual Range<byte> Scale(QueuePriority priority = QueuePriority.Low)
        {
            var r = new Range<byte>();
            switch (priority)
            {
                case QueuePriority.High:
                    r.Minimum = 2;
                    r.Maximum = 5;
                    break;
                case QueuePriority.Medium:
                    r.Minimum = 1;
                    r.Maximum = 4;
                    break;
                default:
                    r.Minimum = 1;
                    r.Maximum = 2;
                    break;
            }

            return r;
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
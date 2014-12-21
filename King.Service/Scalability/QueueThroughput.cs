namespace King.Service.Scalability
{
    using King.Service.Data;

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
        public ushort MessagesPerScaleUnit(QueuePriority priority)
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
        public byte MinimumScale(QueuePriority priority)
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
        /// Maximum Scale
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Maximum Scale</returns>
        public byte MaximumScale(QueuePriority priority)
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
        public byte CheckScaleEvery(QueuePriority priority)
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
        #endregion
    }
}
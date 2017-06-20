namespace King.Service.Data
{
    using global::Azure.Data.Wrappers;
    using King.Service.Scalability;
    using King.Service.Timing;

    /// <summary>
    /// Queue Auto Scaler
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public abstract class QueueAutoScaler<T> : AutoScaler<T>
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="count">Count</param>
        /// <param name="messagesPerScaleUnit">Messages Per-Scale Unit</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="minimum">Minimum Scale</param>
        /// <param name="maximum">Maximmum Scale</param>
        /// <param name="checkScaleInMinutes">Check Scale Every</param>
        public QueueAutoScaler(IQueueCount count, ushort messagesPerScaleUnit = QueueScaler<T>.MessagesPerScaleUnitDefault, T configuration = default(T), byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = BaseTimes.ScaleCheck)
            : base(new QueueScaler<T>(count, messagesPerScaleUnit), configuration, minimum, maximum, checkScaleInMinutes)
        {
        }
        #endregion
    }
}
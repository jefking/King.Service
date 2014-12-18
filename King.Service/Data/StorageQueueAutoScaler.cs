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
    public class StorageQueueAutoScaler<T> : QueueAutoScaler<QueueSetup>
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
        public StorageQueueAutoScaler(IQueueCount count, QueueSetup setup, ushort messagesPerScaleUnit = QueueScaler<T>.MessagesPerScaleUnitDefault, byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = BaseTimes.ScaleCheck)
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
        public override IEnumerable<IScalable> ScaleUnit(QueueSetup setup)
        {
            var processor = setup.Get<T>();
            var dequeue = new StorageDequeueBatchDynamic<T>(setup.Name, setup.ConnectionString, processor);
            switch (setup.Priority)
            {
                case QueuePriority.High:
                case QueuePriority.Medium:
                    yield return new BackoffRunner(dequeue);
                    break;
                case QueuePriority.Low:
                    yield return new AdaptiveRunner(dequeue);
                    break;
                default:
                    throw new InvalidOperationException("Unknown Priority");
            }
        }
        #endregion
    }
}
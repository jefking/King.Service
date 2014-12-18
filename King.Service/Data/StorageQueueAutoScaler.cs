namespace King.Service.Data
{
    using King.Azure.Data;
    using King.Service.Data.Model;
    using System.Collections.Generic;

    /// <summary>
    /// Storage Queue AutoScaler
    /// </summary>
    /// <typeparam name="T">Processor Type</typeparam>
    public class StorageQueueAutoScaler<T> : QueueAutoScaler<QueueSetup>
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="count">Count</param>
        /// <param name="setup">Setup</param>
        public StorageQueueAutoScaler(IQueueCount count, QueueSetup setup)
            : base(count, 100, setup)
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
            yield return new BackoffRunner(dequeue);
        }
        #endregion
    }
}
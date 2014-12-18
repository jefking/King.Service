namespace King.Service.Data.Model
{
    using King.Azure.Data;

    /// <summary>
    /// Queue Setup
    /// </summary>
    public interface IQueueSetup
    {
        #region Properties
        /// <summary>
        /// Name
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Connection String
        /// </summary>
        string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Priority
        /// </summary>
        QueuePriority Priority
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get Processor
        /// </summary>
        /// <typeparam name="T">Dequeue Type</typeparam>
        /// <returns>Processor</returns>
        IProcessor<T> Get<T>();
        #endregion
    }
}
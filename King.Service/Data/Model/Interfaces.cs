namespace King.Service.Data.Model
{
    using King.Azure.Data;

    /// <summary>
    /// Queue Setup Interface
    /// </summary>
    /// <typeparam name="T">Dequeue Type</typeparam>
    public interface IQueueSetup<T>
    {
        #region Properties
        /// <summary>
        /// Name
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Connection String
        /// </summary>
        string ConnectionString
        {
            get;
        }

        /// <summary>
        /// Priority
        /// </summary>
        QueuePriority Priority
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get Processor
        /// </summary>
        /// <returns>Processor</returns>
        IProcessor<T> Get();
        #endregion
    }
}
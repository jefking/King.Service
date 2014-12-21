namespace King.Service.Data
{
    using King.Azure.Data;

    /// <summary>
    /// Queue Setup
    /// </summary>
    /// <typeparam name="T">Dequeue Type</typeparam>
    public abstract class QueueSetup<T> : IQueueSetup<T>
    {
        #region Properties
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Connection String
        /// </summary>
        public virtual string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Priority
        /// </summary>
        public virtual QueuePriority Priority
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get Processor
        /// </summary>
        /// <returns>Processor</returns>
        public abstract IProcessor<T> Get();
        #endregion
    }
}
namespace King.Service.Data.Model
{
    using King.Azure.Data;

    /// <summary>
    /// Queue Setup
    /// </summary>
    public abstract class QueueSetup : IQueueSetup
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
        /// <typeparam name="T">Dequeue Type</typeparam>
        /// <returns>Processor</returns>
        public abstract IProcessor<T> Get<T>();
        #endregion
    }
}
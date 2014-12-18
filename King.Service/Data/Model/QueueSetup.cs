namespace King.Service.Data.Model
{
    using King.Azure.Data;

    /// <summary>
    /// Queue Setup
    /// </summary>
    public class QueueSetup : IQueueSetup
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
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IProcessor<T> Get<T>()
        {
            return null;
        }
        #endregion
    }
}
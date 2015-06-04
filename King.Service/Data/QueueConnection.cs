namespace King.Service.Data
{
    /// <summary>
    /// Queue Connection
    /// </summary>
    /// <typeparam name="T">Data Type</typeparam>
    public class QueueConnection<T> : IQueueConnection<T>
    {
        #region Properties
        /// <summary>
        /// Connection String
        /// </summary>
        public string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Queue Setup
        /// </summary>
        public virtual IQueueSetup<T> Setup
        {
            get;
            set;
        }
        #endregion
    }
}
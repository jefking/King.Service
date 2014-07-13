namespace King.Azure.BackgroundWorker.Data
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Queue Poller
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class QueuePoller<T> : IPoller<T>
    {
        #region Members
        /// <summary>
        /// Queue
        /// </summary>
        private readonly IQueue queue = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="queueName">Queue Name</param>
        /// <param name="connectionString">Connection String</param>
        public QueuePoller(string queueName, string connectionString)
            : this(new Queue(queueName, connectionString))
        {
        }

        /// <summary>
        /// Constructor for Mocking
        /// </summary>
        /// <param name="queue">Queue</param>
        public QueuePoller(IQueue queue)
        {
            if (null == queue)
            {
                throw new ArgumentNullException("queue");
            }

            this.queue = queue;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Poll for Queued Message
        /// </summary>
        /// <returns>Queued Item</returns>
        public async Task<IQueued<T>> Poll()
        {
            var msg = await this.queue.Get();
            return null != msg ? new QueuedMessage<T>(this.queue, msg) : null;
        }
        #endregion
    }
}
namespace King.Azure.BackgroundWorker.Data
{
    using System;
    using System.Threading.Tasks;

    public class QueuePoller<T> : IPoller<T>
    {
        #region Members
        private readonly IQueue queue = null;
        #endregion

        #region Constructors
        public QueuePoller(string queueName, string connectionString)
            : this(new Queue(queueName, connectionString))
        {
        }
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
        public async Task<IQueued<T>> Poll()
        {
            var msg = await this.queue.Get();
            return null != msg ? new QueuedMessage<T>(this.queue, msg) : null;
        }
        #endregion
    }
}
namespace King.Service.ServiceBus.Queue
{
    using King.Azure.Data;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Generic Poller, re-use for any queued models you have.
    /// </summary>
    /// <typeparam name="T">Message with T as Body</typeparam>
    public class ServiceBusQueuePoller<T> : IPoller<T>
    {
        #region Members
        /// <summary>
        /// Queue Client
        /// </summary>
        private readonly QueueClient client;
        #endregion

        #region Constructors
        public ServiceBusQueuePoller(QueueClient client)
        {
            if (null == client)
            {
                throw new ArgumentNullException("client");
            }

            this.client = client;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Poll for Queued Message
        /// </summary>
        /// <returns>Queued Item</returns>
        public async Task<IQueued<T>> Poll()
        {
            var msg = await this.client.ReceiveAsync();
            return null == msg ? null : new Queued<T>(msg);
        }

        public async Task<System.Collections.Generic.IEnumerable<IQueued<T>>> PollMany(int messageCount = 5)
        {
            var msgs = await this.client.ReceiveBatchAsync(messageCount);
            return null == msgs || !msgs.Any() ? null : msgs.Select(m => new Queued<T>(m));
        }
        #endregion
    }
}
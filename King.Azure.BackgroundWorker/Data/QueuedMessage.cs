namespace King.Azure.BackgroundWorker.Data
{
    using Microsoft.WindowsAzure.Storage.Queue;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Queued Message
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueuedMessage<T> : IQueued<T>
    {
        #region Members
        /// <summary>
        /// Storage Queue
        /// </summary>
        private readonly IStorageQueue queue = null;

        /// <summary>
        /// Cloud Queue Message
        /// </summary>
        private readonly CloudQueueMessage message = null;
        #endregion

        #region Constructors
        public QueuedMessage(IStorageQueue queue, CloudQueueMessage message)
        {
            if (null == queue)
            {
                throw new ArgumentNullException("queue");
            }
            if (null == message)
            {
                throw new ArgumentNullException("message");
            }

            this.queue = queue;
            this.message = message;
        }
        #endregion

        #region Methods
        public async Task Delete()
        {
            await this.queue.Delete(this.message);
        }

        public Task Abandon()
        {
            return null; //No Abandon?
        }

        public async Task<T> Data()
        {
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(this.message.AsString));
        }
        #endregion
    }
}
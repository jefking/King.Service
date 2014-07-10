namespace King.Azure.BackgroundWorker.Data
{
    using Microsoft.WindowsAzure.Storage.Queue;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Queue
    /// </summary>
    public class Queue : AzureStorage, IQueue
    {
        #region Members
        /// <summary>
        /// Client
        /// </summary>
        private CloudQueueClient client;

        /// <summary>
        /// Reference
        /// </summary>
        private CloudQueue reference;
        #endregion

        #region Constructors
        /// <summary>
        /// Queue
        /// </summary>
        /// <param name="name">Name</param>
        public Queue(string name, string connectionStringKey)
            : base(connectionStringKey)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }

            this.client = base.account.CreateCloudQueueClient();
            this.reference = client.GetQueueReference(name);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Table Name
        /// </summary>
        public string Name
        {
            get
            {
                return this.reference.Name;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create If Not Exists
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateIfNotExists()
        {
            return await this.reference.CreateIfNotExistsAsync();
        }

        /// <summary>
        /// Get Cloud Queue Message
        /// </summary>
        /// <returns>Message</returns>
        public async Task<CloudQueueMessage> Get()
        {
            return await this.reference.GetMessageAsync();
        }

        /// <summary>
        /// Save Message to Queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        public async Task Save(CloudQueueMessage message)
        {
            if (null == message)
            {
                throw new ArgumentNullException("message");
            }

            await this.reference.AddMessageAsync(message);
        }

        /// <summary>
        /// Delete Message from Queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        public async Task Delete(CloudQueueMessage message)
        {
            if (null == message)
            {
                throw new ArgumentNullException("message");
            }

            await this.reference.DeleteMessageAsync(message);
        }
        #endregion
    }
}

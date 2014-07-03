namespace King.Azure.BackgroundWorker.Data
{
    using Microsoft.WindowsAzure.Storage.Queue;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Queue
    /// </summary>
    public class Queue : AzureStorage, IAzureStorage
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
        #endregion
    }
}

namespace King.Azure.BackgroundWorker.Data
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Table Storage
    /// </summary>
    /// <remarks>
    /// http://www.windowsazure.com/en-us/develop/net/how-to-guides/table-services/
    /// </remarks>
    public class TableStorage : AzureStorage, ITableStorage
    {
        #region Members
        /// <summary>
        /// Table Client
        /// </summary>
        private CloudTableClient client;

        /// <summary>
        /// Table
        /// </summary>
        private CloudTable reference;
        #endregion

        #region Constructors
        /// <summary>
        /// Table Storage
        /// </summary>
        /// <param name="tableName">Table Name</param>
        public TableStorage(string tableName, string connectionStringKey)
            : base(connectionStringKey)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException("tableName");
            }

            this.client = base.account.CreateCloudTableClient();
            this.reference = client.GetTableReference(tableName);
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
        /// Create Table
        /// </summary>
        /// <param name="tableName">Table Name</param>
        public async Task<bool> Create()
        {
            return await this.reference.CreateIfNotExistsAsync();
        }

        /// <summary>
        /// Delete Table
        /// </summary>
        /// <param name="tableName"></param>
        public async Task Delete()
        {
            await this.reference.DeleteAsync();
        }

        /// <summary>
        /// Insert or update the record in table
        /// </summary>
        /// <param name="item">Scheduled Task Entry</param>
        public async Task<TableResult> InsertOrReplace(ITableEntity entry)
        {
            var insertOperation = TableOperation.InsertOrReplace(entry);
            return await this.reference.ExecuteAsync(insertOperation);
        }

        /// <summary>
        /// Insert Batch
        /// </summary>
        /// <param name="entities"></param>
        public async Task<IEnumerable<TableResult>> Insert(IEnumerable<ITableEntity> entities)
        {
            var batchOperation = new TableBatchOperation();
            foreach (var entity in entities)
            {
                batchOperation.InsertOrMerge(entity);
            }

            return await this.reference.ExecuteBatchAsync(batchOperation);
        }

        /// <summary>
        /// Query By Partition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="partition"></param>
        /// <returns></returns>
        public IEnumerable<T> QueryByPartition<T>(string partition)
            where T : ITableEntity, new()
        {
            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partition));
            return this.reference.ExecuteQuery(query);
        }
        #endregion
    }
}
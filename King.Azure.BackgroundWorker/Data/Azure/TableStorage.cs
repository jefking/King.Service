namespace King.Azure.BackgroundWorker.Data.Azure
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage.Table;

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
        private CloudTable table;
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
            this.table = client.GetTableReference(tableName);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create If Not Exists
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateIfNotExists()
        {
            return await this.table.CreateIfNotExistsAsync();
        }

        /// <summary>
        /// Create Table
        /// </summary>
        /// <param name="tableName">Table Name</param>
        public async Task<bool> Create()
        {
            return await this.table.CreateIfNotExistsAsync();
        }

        /// <summary>
        /// Delete Table
        /// </summary>
        /// <param name="tableName"></param>
        public async Task Delete()
        {
            await this.table.DeleteAsync();
        }

        /// <summary>
        /// Insert or update the record in table
        /// </summary>
        /// <param name="item">Scheduled Task Entry</param>
        public async Task<TableResult> InsertOrReplace(ITableEntity entry)
        {
            var insertOperation = TableOperation.InsertOrReplace(entry);
            return await this.table.ExecuteAsync(insertOperation);
        }

        /// <summary>
        /// Insert Batch
        /// </summary>
        /// <param name="entities"></param>
        public async Task<IEnumerable<TableResult>> Insert(IEnumerable<TableEntity> entities)
        {
            var batchOperation = new TableBatchOperation();
            foreach (var entity in entities)
            {
                batchOperation.InsertOrMerge(entity);
            }

            return await this.table.ExecuteBatchAsync(batchOperation);
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
            return this.table.ExecuteQuery(query);
        }
        #endregion
    }
}
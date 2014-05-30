namespace King.Service.Azure
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
    public class TableStorage : AzureStorage
    {
        #region Members
        /// <summary>
        /// Table Name
        /// </summary>
        private readonly string tableName;
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

            this.tableName = tableName;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create Table
        /// </summary>
        /// <param name="tableName">Table Name</param>
        public async Task<bool> Create()
        {
            var tableClient = account.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            return await table.CreateIfNotExistsAsync();
        }

        /// <summary>
        /// Delete Table
        /// </summary>
        /// <param name="tableName"></param>
        public async Task Delete()
        {
            var tableClient = account.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            await table.DeleteAsync();
        }

        /// <summary>
        /// Insert or update the record in table
        /// </summary>
        /// <param name="item">Scheduled Task Entry</param>
        public async Task<TableResult> InsertOrReplace(ITableEntity entry)
        {
            var tableClient = account.CreateCloudTableClient();
            var table = tableClient.GetTableReference(this.tableName);
            var insertOperation = TableOperation.InsertOrReplace(entry);
            return await table.ExecuteAsync(insertOperation);
        }

        /// <summary>
        /// Insert Batch
        /// </summary>
        /// <param name="entities"></param>
        public async Task<IEnumerable<TableResult>> Insert(IEnumerable<TableEntity> entities)
        {
            var tableClient = account.CreateCloudTableClient();
            var table = tableClient.GetTableReference(this.tableName);
            var batchOperation = new TableBatchOperation();
            foreach (var entity in entities)
            {
                batchOperation.InsertOrMerge(entity);
            }

            return await table.ExecuteBatchAsync(batchOperation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="partition"></param>
        /// <returns></returns>
        public IEnumerable<T> QueryByPartition<T>(string partition)
            where T : ITableEntity, new()
        {
            var tableClient = account.CreateCloudTableClient();
            var table = tableClient.GetTableReference(this.tableName);
            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partition));
            return table.ExecuteQuery(query);
        }
        #endregion
    }
}
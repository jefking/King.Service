namespace King.Azure.BackgroundWorker.Data
{
    using Microsoft.WindowsAzure.Storage.Queue;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Scheduled Task Core for Mocking
    /// </summary>
    public interface ICoordinator
    {
        #region Methods
        /// <summary>
        /// Initialize Table
        /// </summary>
        TaskManager InitializeTask();

        /// <summary>
        /// Determine whether a new task needs to be executed
        /// </summary>
        /// <param name="entry">Scheduled Task Entry</param>
        /// <returns>True if need to execute, false if not</returns>
        bool Check(Type type);

        /// <summary>
        /// Start Task
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        Task Start(Type type, Guid identifier, DateTime start);

        /// <summary>
        /// Complete Task
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        Task Complete(Type type, Guid identifier, DateTime start, DateTime end, bool success);
        #endregion

        #region Properties
        int PeriodInSeconds
        {
            get;
        }
        #endregion
    }

    /// <summary>
    /// Table Storage Interface
    /// </summary>
    public interface ITableStorage : IAzureStorage
    {
        #region Methods
        /// <summary>
        /// Create Table
        /// </summary>
        /// <param name="tableName">Table Name</param>
        Task<bool> Create();

        /// <summary>
        /// Delete Table
        /// </summary>
        /// <param name="tableName"></param>
        Task Delete();

        /// <summary>
        /// Insert or update the record in table
        /// </summary>
        /// <param name="item">Scheduled Task Entry</param>
        Task<TableResult> InsertOrReplace(ITableEntity entry);

        /// <summary>
        /// Insert Batch
        /// </summary>
        /// <param name="entities"></param>
        Task<IEnumerable<TableResult>> Insert(IEnumerable<ITableEntity> entities);

        /// <summary>
        /// Query By Partition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="partition"></param>
        /// <returns></returns>
        IEnumerable<T> QueryByPartition<T>(string partition)
            where T : ITableEntity, new();
        #endregion
    }

    /// <summary>
    /// IQueue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueue<T>
    {
        #region Methods
        /// <summary>
        /// Pop Cloud Queue Message
        /// </summary>
        /// <returns>Message</returns>
        Task<T> Get();

        /// <summary>
        /// Delete Message from Queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        Task Delete(T message);

        /// <summary>
        /// Save Message to Queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        Task Save(T message);
        #endregion
    }

    /// <summary>
    /// IStorage Queue
    /// </summary>
    public interface IStorageQueue : IQueue<CloudQueueMessage>, IAzureStorage
    {
    }

    /// <summary>
    /// Azure Storage
    /// </summary>
    public interface IAzureStorage
    {
        #region Properties
        string Name
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create If Not Exists
        /// </summary>
        /// <returns></returns>
        Task<bool> CreateIfNotExists();
        #endregion
    }

    /// <summary>
    /// IProcessor
    /// </summary>
    public interface IProcessor<T>
    {
        #region Methods
        /// <summary>
        /// Process Data
        /// </summary>
        /// <param name="data">Data to Process</param>
        /// <returns>Successful</returns>
        Task<bool> Process(T data);
        #endregion
    }

    /// <summary>
    /// IPoller
    /// </summary>
    public interface IPoller<T>
    {
        #region Methods
        /// <summary>
        /// Poll for Queued Message
        /// </summary>
        /// <returns>Queued Item</returns>
        Task<IQueued<T>> Poll();
        #endregion
    }

    /// <summary>
    /// IQueued
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueued<T>
    {
        #region Methods
        /// <summary>
        /// Delete Message
        /// </summary>
        /// <returns>Task</returns>
        Task Delete();

        /// <summary>
        /// Abandon Message
        /// </summary>
        /// <returns>Task</returns>
        Task Abandon();

        /// <summary>
        /// Data
        /// </summary>
        /// <returns>Data</returns>
        Task<T> Data();
        #endregion
    }
}
namespace King.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    #region IStarter
    /// <summary>
    /// Runnable Tasks Interface
    /// </summary>
    public interface IStarter : IDisposable
    {
        #region Methods
        /// <summary>
        /// Runs Service
        /// </summary>
        void Start(IEnumerable<IRunnable> tasks);
        #endregion
    }
    #endregion

    #region IRunnable
    /// <summary>
    /// Runnable Tasks Interface
    /// </summary>
    public interface IRunnable : IDisposable
    {
        #region Methods
        /// <summary>
        /// Stops Service
        /// </summary>
        /// <returns>Stopped</returns>
        bool Stop();

        /// <summary>
        /// Runs Service
        /// </summary>
        /// <returns>Running</returns>
        bool Start();
        #endregion
    }
    #endregion

    #region IScalable
    /// <summary>
    /// Scalable Task Interface
    /// </summary>
    public interface IScalable : IRunnable
    {
        #region Properties
        /// <summary>
        /// Service Should Scale
        /// </summary>
        bool Scale
        {
            get;
        }
        #endregion
    }
    #endregion

    #region IDynamicRuns
    /// <summary>
    /// Dynamic Runs Interface
    /// </summary>
    public interface IDynamicRuns : IRuns
    {
        #region Properties
        /// <summary>
        /// Maximum Period In Seconds
        /// </summary>
        int MaximumPeriodInSeconds
        {
            get;
        }
        #endregion
    }
    #endregion

    #region IRuns
    /// <summary>
    /// Dynamic Runs Interface
    /// </summary>
    public interface IRuns
    {
        #region Methods
        /// <summary>
        /// Run
        /// </summary>
        /// <returns>Work was done.</returns>
        Task<bool> Run();
        #endregion

        #region Properties
        /// <summary>
        /// Minimum Period In Seconds
        /// </summary>
        int MinimumPeriodInSeconds
        {
            get;
        }
        #endregion
    }
    #endregion

    #region ITaskFactory
    /// <summary>
    /// Task Factory Interface
    /// </summary>
    /// <typeparam name="T">Passthrough</typeparam>
    public interface ITaskFactory<T>
    {
        #region Methods
        /// <summary>
        /// Build Task Manifest
        /// </summary>
        /// <param name="passthrough">Configuration</param>
        /// <returns>Runnable Tasks</returns>
        IEnumerable<IRunnable> Tasks(T passthrough);
        #endregion
    }
    #endregion

    #region IRoleTaskManager
    /// <summary>
    /// Role Task Manager Interface
    /// </summary>
    public interface IRoleTaskManager<T> : IDisposable
    {
        #region Properties
        /// <summary>
        /// Tasks
        /// </summary>
        IReadOnlyCollection<IRunnable> Tasks
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run
        /// </summary>
        void Run();

        /// <summary>
        /// On Start
        /// </summary>
        /// <param name="passthrough">Passthrough</param>
        /// <returns>Started</returns>
        bool OnStart(T passthrough = default(T));

        /// <summary>
        /// On Stop
        /// </summary>
        void OnStop();
        #endregion
    }
    #endregion

    #region IQueued
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
        Task Complete();

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
    #endregion

    #region IProcessor
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
    #endregion

    #region IPoller
    /// <summary>
    /// Store Poller Interface
    /// </summary>
    /// <typeparam name="T">Dequeue Type</typeparam>
    public interface IPoller<T>
    {
        #region Methods
        /// <summary>
        /// Poll for Queued Message
        /// </summary>
        /// <returns>Queued Item</returns>
        Task<IQueued<T>> Poll();

        /// <summary>
        /// Poll for Queued Message
        /// </summary>
        /// <returns>Queued Item</returns>
        Task<IEnumerable<IQueued<T>>> PollMany(int messageCount = 5);
        #endregion
    }
    #endregion

    #region IAzureStorage
    /// <summary>
    /// Azure Storage
    /// </summary>
    public interface IAzureStorage
    {
        #region Properties
        /// <summary>
        /// Name
        /// </summary>
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

        /// <summary>
        /// Delete Item
        /// </summary>
        /// <returns>Task</returns>
        Task Delete();
        #endregion
    }
    #endregion

    #region IQueueCount
    /// <summary>
    /// Queue Count
    /// </summary>
    public interface IQueueCount
    {
        #region Methods
        /// <summary>
        /// Approixmate Message Count
        /// </summary>
        /// <returns>Message Count</returns>
        Task<long?> ApproixmateMessageCount();
        #endregion
    }
    #endregion
}
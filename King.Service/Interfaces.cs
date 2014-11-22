namespace King.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    #region IRunnable
    /// <summary>
    /// Runnable interface for Tasks
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
    /// Task Factory
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
}
namespace King.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Runnable interface for Tasks
    /// </summary>
    public interface IRunnable
    {
        #region Methods
        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="state">State of Timer</param>
        void Run(object state);

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

    /// <summary>
    /// Dynamic Runs Interface
    /// </summary>
    public interface IDynamicRuns
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

        /// <summary>
        /// Maximum Period In Seconds
        /// </summary>
        int MaximumPeriodInSeconds
        {
            get;
        }
        #endregion
    }

    /// <summary>
    /// Task Factory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITaskFactory<T>
    {
        #region Methods
        IEnumerable<IRunnable> Tasks(T passthrough);
        #endregion
    }

    /// <summary>
    /// Role Task Manager Interface
    /// </summary>
    public interface IRoleTaskManager<T>
    {
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
}
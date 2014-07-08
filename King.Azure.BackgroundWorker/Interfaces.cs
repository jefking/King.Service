namespace King.Azure.BackgroundWorker
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
    /// Task Factory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITaskFactory<T>
    {
        #region Methods
        IEnumerable<IRunnable> Tasks(T passthrough);
        #endregion
    }

    public interface IRoleServiceManager
    {
        #region Methods
        /// <summary>
        /// Run
        /// </summary>
        void Run();

        /// <summary>
        /// On Start
        /// </summary>
        /// <returns>Started</returns>
        bool OnStart();

        /// <summary>
        /// On Stop
        /// </summary>
        void OnStop();
        #endregion
    }

    public interface ITiming
    {
        double Exponential(int min = 0, int max = 60, int attempts = 0);
    }
}
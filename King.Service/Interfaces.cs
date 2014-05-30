namespace King.Service
{
    using System.Collections.Generic;

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
        void Execute(object state);

        /// <summary>
        /// Stops Service
        /// </summary>
        /// <returns>Stopped</returns>
        bool Stop();

        /// <summary>
        /// Runs Service
        /// </summary>
        /// <returns>Running</returns>
        bool Run();
        #endregion
    }

    /// <summary>
    /// Service Factory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IServiceFactory<T>
    {
        IEnumerable<IRunnable> Services(T passthrough);
    }
}
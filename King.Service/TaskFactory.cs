namespace King.Service
{
    using System.Collections.Generic;

    /// <summary>
    /// Service Factory
    /// </summary>
    public abstract class TaskFactory : ITaskFactory<object>
    {
        #region Methods
        /// <summary>
        /// Tasks
        /// </summary>
        /// <param name="passthrough">Passthrough</param>
        /// <returns>Runnable Services</returns>
        public abstract IEnumerable<IRunnable> Tasks(object passthrough);
        #endregion
    }
}
namespace King.Service
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Service Factory
    /// </summary>
    [Obsolete("Just use interface King.Service.ITaskFactory<T>")]
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
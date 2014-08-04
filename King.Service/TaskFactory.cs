namespace King.Service
{
    using System.Collections.Generic;

    /// <summary>
    /// Service Factory
    /// </summary>
    public abstract class TaskFactory : ITaskFactory<object>
    {
        #region Methods
        public abstract IEnumerable<IRunnable> Tasks(object passthrough);
        #endregion
    }
}
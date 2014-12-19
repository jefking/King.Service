namespace King.Service
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Timing;
    using System.Collections.Generic;

    /// <summary>
    /// Task Factory
    /// </summary>
    /// <typeparam name="T">Passthrough</typeparam>
    public abstract class TaskFactory<T> : ITaskFactory<T>
    {
        #region Methods
        /// <summary>
        /// Tasks
        /// </summary>
        /// <typeparam name="Y"></typeparam>
        /// <param name="factory"></param>
        /// <param name="passthrough"></param>
        /// <returns></returns>
        public IEnumerable<IRunnable> Tasks<Y>(ITaskFactory<Y> factory, Y passthrough)
        {
            foreach (var t in factory.Tasks(passthrough))
            {
                yield return t;
            }
        }

        /// <summary>
        /// Initialize Storage Task
        /// </summary>
        /// <param name="storage">Azure Storage</param>
        /// <returns>Runnable</returns>
        public virtual IRunnable InitializeStorage(IAzureStorage storage)
        {
            return new InitializeStorageTask(storage);
        }

        /// <summary>
        /// Adaptive Runner Task
        /// </summary>
        /// <param name="runs">Dynamic Runs</param>
        /// <returns>Runnable</returns>
        public virtual IRunnable Adaptive(IDynamicRuns runs, Strategy strategy = Strategy.Exponential)
        {
            return new AdaptiveRunner(runs, strategy);
        }

        /// <summary>
        /// Backoff Runner Task
        /// </summary>
        /// <param name="runs">Dynamic Runs</param>
        /// <returns>Runnable</returns>
        public virtual IRunnable Backoff(IDynamicRuns runs, Strategy strategy = Strategy.Exponential)
        {
            return new BackoffRunner(runs, strategy);
        }

        /// <summary>
        /// Default Recurring Task
        /// </summary>
        /// <param name="runs">Runs</param>
        /// <returns></returns>
        public virtual IRunnable DefaultRecurring(IRuns runs)
        {
            return new RecurringRunner(runs);
        }

        /// <summary>
        /// Tasks
        /// </summary>
        /// <param name="passthrough">Passthrough</param>
        /// <returns>Tasks</returns>
        public abstract IEnumerable<IRunnable> Tasks(T passthrough);
        #endregion
    }
}
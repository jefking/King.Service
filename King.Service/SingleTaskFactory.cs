namespace King.Service
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Create Single Task, perfect for micro-services
    /// </summary>
    /// <typeparam name="T">Passthrough</typeparam>
    public class SingleTaskFactory<T> : ITaskFactory<T>
    {
        #region Members
        /// <summary>
        /// Creates Task on Demand
        /// </summary>
        protected readonly Func<T, IRunnable> create;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="create">Function for Creation</param>
        public SingleTaskFactory(Func<T, IRunnable> create)
        {
            if (null == create)
            {
                throw new ArgumentNullException("create is null.");
            }

            this.create = create;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Build Task Manifest
        /// </summary>
        /// <param name="passthrough">Configuration</param>
        /// <returns>Runnable Tasks</returns>
        public IEnumerable<IRunnable> Tasks(T passthrough)
        {
            yield return this.create(passthrough);
        }
        #endregion
    }
}
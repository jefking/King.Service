namespace King.Service.Data
{
    using System;
    using global::Azure.Data.Wrappers;
    using System.Threading.Tasks;

    /// <summary>
    /// Action Processor, takes action on model
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public class ActionProcessor<T> : IProcessor<T>
    {
        #region Members
        /// <summary>
        /// Action to take on model
        /// </summary>
        protected readonly QueueActivity<T> action;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="action">Action</param>
        public ActionProcessor(QueueActivity<T> action)
        {
            if (null == action)
            {
                throw new ArgumentNullException("action");
            }

            this.action = action;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Process
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>async Task</returns>
        public virtual Task<bool> Process(T data)
        {
            var handle = this.action;
            if (null != handle)
            {
                return handle(data);
            }
            else
            {
                throw new InvalidOperationException("Action null, activity to run.");
            }
        }
        #endregion
    }
}
namespace King.Service.Data
{
    using System;
    using global::Azure.Data.Wrappers;

    /// <summary>
    /// Queue Setup
    /// </summary>
    /// <typeparam name="T">Dequeue Type</typeparam>
    public class QueueSetup<T> : IQueueSetup<T>
    {
        #region Properties
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }
        
        /// <summary>
        /// Priority
        /// </summary>
        public virtual QueuePriority Priority
        {
            get;
            set;
        }

        /// <summary>
        /// Processor
        /// </summary>
        public virtual Func<IProcessor<T>> Processor
        {
            get;
            set;
        }
        #endregion
    }
}
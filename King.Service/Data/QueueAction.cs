namespace King.Service.Data
{
    using King.Azure.Data;
    using System;

    /// <summary>
    /// Queue Action
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public class QueueAction<T> : QueueSetup<T>
    {
        #region Properties
        /// <summary>
        /// Queue Activity
        /// </summary>
        public virtual QueueActivity<T> Activity
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new Processor
        /// </summary>
        public override Func<IProcessor<T>> Processor
        {
            get
            {
                return () => { return new ActionProcessor<T>(this.Activity); };
            }
        }
        #endregion
    }
}
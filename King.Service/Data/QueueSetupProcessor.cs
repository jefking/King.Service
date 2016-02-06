namespace King.Service.Data
{
    using Azure.Data;
    using System;

    /// <summary>
    /// Queue Setup Processor
    /// </summary>
    /// <typeparam name="Y">Processor</typeparam>
    /// <typeparam name="T">Model</typeparam>
    public class QueueSetupProcessor<Y, T> : QueueSetup<T>
        where Y : IProcessor<T>, new()
    {
        #region Properties
        /// <summary>
        /// Processor
        /// </summary>
        public override Func<IProcessor<T>> Processor
        {
            get
            {
                return () => { return new Y(); };
            }
        }
        #endregion
    }
}
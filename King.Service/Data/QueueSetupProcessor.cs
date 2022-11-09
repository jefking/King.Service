namespace King.Service.Data
{
    using System;

    /// <summary>
    /// Queue Setup Processor
    /// </summary>
    /// <typeparam name="Y">Processor</typeparam>
    /// <typeparam name="X">Model</typeparam>
    public class QueueSetupProcessor<Y, X> : QueueSetup<X>
        where Y : IProcessor<X>, new()
    {
        #region Properties
        /// <summary>
        /// Processor
        /// </summary>
        public override Func<IProcessor<X>> Processor
        {
            get
            {
                return () => { return new Y(); };
            }
        }
        #endregion
    }
}
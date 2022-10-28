namespace King.Service.Data
{
    using global::Azure.Data.Wrappers;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Processor Function
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProcessorFunction<T> : IProcessor<T>
    {
        #region Members
        /// <summary>
        /// Function
        /// </summary>
        protected readonly Func<object, bool> func;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="func">Function</param>
        public ProcessorFunction(Func<object, bool> func)
        {
            if (null == func)
            {
                throw new ArgumentNullException("func");
            }

            this.func = func;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Process
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>Task, bool result</returns>
        public virtual async Task<bool> Process(T data)
        {
            var factory = new TaskFactory();
            return await factory.StartNew(this.func, data);
        }
        #endregion
    }
}
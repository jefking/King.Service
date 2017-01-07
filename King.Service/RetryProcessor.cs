namespace King.Service
{
    using King.Azure.Data;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Retry Processor, Wrapper for Processors
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public class RetryProcessor<T> : IProcessor<T>
    {
        #region Members
        /// <summary>
        /// Processor
        /// </summary>
        private readonly IProcessor<T> processor = null;

        /// <summary>
        /// Maximum number of attempts (safety)
        /// </summary>
        private readonly byte countMax = 3;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="processor">processor</param>
        public RetryProcessor(IProcessor<T> processor, byte countMax = 3)
        {
            if (null == processor)
            {
                throw new ArgumentNullException("processor");
            }
            if (0 >= countMax)
            {
                throw new ArgumentException("countMax: must be greater than 0.");
            }

            this.processor = processor;
            this.countMax = countMax;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Process
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>Success</returns>
        public async Task<bool> Process(T data)
        {
            var success = false;
            var count = 0;

            while (!success && count < this.countMax)
            {
                try
                {
                    success = await this.processor.Process(data);
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString());

                    success = false;
                }

                count++;
            }

            return success;
        }
        #endregion
    }
}
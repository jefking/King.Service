namespace King.Service
{
    using King.Azure.Data;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RetryProcessor<T> : IProcessor<T>
    {
        #region Members
        /// <summary>
        /// 
        /// </summary>
        private readonly IProcessor<T> processor = null;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="processor"></param>
        public RetryProcessor(IProcessor<T> processor)
        {
            if (null == processor)
            {
                throw new ArgumentNullException("processor");
            }

            this.processor = processor;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> Process(T data)
        {
            var success = false;
            var count = 0;

            while (!success && count < 3)
            {

                try
                {
                    success = await this.processor.Process(data);
                }
                catch (Exception ex)
                {
                    success = false;
                }

                count++;
            }

            return success;
        }
        #endregion
    }
}
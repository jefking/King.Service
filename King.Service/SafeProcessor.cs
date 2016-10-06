namespace King.Service
{
    using King.Azure.Data;
    using System.Threading.Tasks;

    /// <summary>
    /// Initializes Your Processor for every Process call, and aquires lock for thread safety
    /// </summary>
    /// <remarks>
    /// This was developed as Entity Framework required single-threaded access; while we were porting code onto King.Service
    /// </remarks>
    /// <typeparam name="Y">Processor</typeparam>
    /// <typeparam name="X">Model</typeparam>
    public class SafeProcessor<Y, X> : IProcessor<X>
        where Y : IProcessor<X>, new()
    {
        #region Members
        /// <summary>
        /// Lock Object
        /// </summary>
        protected readonly static object lockObject = new object();
        #endregion

        #region Methods
        /// <summary>
        /// Process Data
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>Work was Done</returns>
        public async Task<bool> Process(X data)
        {
            bool result;
            lock (lockObject)
            {
                var y = new Y();
                result = y.Process(data).Result;
            }

            return await Task.FromResult(result);
        }
        #endregion
    }
}
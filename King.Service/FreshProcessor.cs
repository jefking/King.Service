namespace King.Service
{
    using King.Azure.Data;
    using System.Threading.Tasks;

    /// <summary>
    /// Initializes Your Processor for every Process call
    /// </summary>
    /// <typeparam name="Y">Processor</typeparam>
    /// <typeparam name="X">Model</typeparam>
    public class FreshProcessor<Y, X> : IProcessor<X>
        where Y : IProcessor<X>, new()
    {
        #region Methods
        /// <summary>
        /// Process Data
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>Work was Done</returns>
        public async Task<bool> Process(X data)
        {
            var y = new Y();
            return await y.Process(data);
        }
        #endregion
    }
}
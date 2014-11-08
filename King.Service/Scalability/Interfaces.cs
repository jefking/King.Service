namespace King.Service.Scalability
{
    using System.Collections.Concurrent;

    /// <summary>
    /// Scaler Interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IScaler<T>
    {
        #region Methods
        /// <summary>
        /// Scale Up by one unit
        /// </summary>
        /// <param name="factory">Task Factory</param>
        /// <param name="units">Units</param>
        /// <param name="serviceName">Service Name (for Tracing)</param>
        void ScaleUp(ITaskFactory<T> factory, ConcurrentStack<IRoleTaskManager<T>> units, string serviceName);

        /// <summary>
        /// Scale down by one unit
        /// </summary>
        /// <param name="units">Units</param>
        /// <param name="serviceName">Service Name (for Tracing)</param>
        void ScaleDown(ConcurrentStack<IRoleTaskManager<T>> units, string serviceName);
        #endregion
    }
}
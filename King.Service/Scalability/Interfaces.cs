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
        /// Is First Run
        /// </summary>
        /// <param name="minimum">Minimum</param>
        /// <param name="units">Units</param>
        /// <returns>Is First Run</returns>
        bool IsFirstRun(byte minimum, ConcurrentStack<IRoleTaskManager<T>> units);

        /// <summary>
        /// Initialize Scale
        /// </summary>
        /// <param name="minimum">Minimum</param>
        /// <param name="factory">Factory</param>
        /// <param name="units">Units</param>
        /// <param name="serviceName">Service Name</param>
        void Initialize(byte minimum, ITaskFactory<T> factory, ConcurrentStack<IRoleTaskManager<T>> units, string serviceName);

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
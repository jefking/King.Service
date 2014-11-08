namespace King.Service.Scalability
{
    using System.Collections.Concurrent;

    /// <summary>
    /// Scaler Interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IScaler<T>
    {
        #region Properties
        /// <summary>
        /// Currnet Unit Count
        /// </summary>
        int CurrentUnits
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Should Scale
        /// </summary>
        /// <returns>Scale Direction</returns>
        Direction ShouldScale();

        /// <summary>
        /// Is First Run
        /// </summary>
        /// <param name="minimum">Minimum</param>
        /// <returns>Is First Run</returns>
        bool IsFirstRun(byte minimum);

        /// <summary>
        /// Initialize Scale
        /// </summary>
        /// <param name="minimum">Minimum</param>
        /// <param name="factory">Factory</param>
        /// <param name="taskName">Task Name</param>
        void Initialize(byte minimum, ITaskFactory<T> factory, string taskName);

        /// <summary>
        /// Scale Up by one unit
        /// </summary>
        /// <param name="factory">Task Factory</param>
        /// <param name="taskName">Task Name</param>
        void ScaleUp(ITaskFactory<T> factory, string taskName);

        /// <summary>
        /// Scale down by one unit
        /// </summary>
        /// <param name="taskName">Task Name</param>
        void ScaleDown(string taskName);
        #endregion
    }
}
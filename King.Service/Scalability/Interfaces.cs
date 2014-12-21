namespace King.Service.Scalability
{
    using King.Service.Data;
    using System;

    #region IScaler
    /// <summary>
    /// Scaler Interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IScaler<T> : IDisposable
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
        bool? ShouldScale();

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
        /// <param name="configuration">Configuration</param>
        /// <param name="taskName">Task Name</param>
        void Initialize(byte minimum, ITaskFactory<T> factory, T configuration, string taskName);

        /// <summary>
        /// Scale Up by one unit
        /// </summary>
        /// <param name="factory">Factory</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="taskName">Task Name</param>
        void ScaleUp(ITaskFactory<T> factory, T configuration, string taskName);

        /// <summary>
        /// Scale down by one unit
        /// </summary>
        /// <param name="taskName">Task Name</param>
        void ScaleDown(string taskName);
        #endregion
    }
    #endregion

    #region IQueueThroughput
    /// <summary>
    /// Queue Throughput Interface
    /// </summary>
    public interface IQueueThroughput
    {
        #region Methods
        /// <summary>
        /// Messages per Scale-unit
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Number of messages per scale unit</returns>
        ushort MessagesPerScaleUnit(QueuePriority priority);

        /// <summary>
        /// Minimum Scale
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Minimum Scale</returns>
        byte MinimumScale(QueuePriority priority);

        /// <summary>
        /// Maximum Scale
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Maximum Scale</returns>
        byte MaximumScale(QueuePriority priority);

        /// <summary>
        /// Check Scale every (in minutes)
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Check Scale every (in minutes)</returns>
        byte CheckScaleEvery(QueuePriority priority);

        /// <summary>
        /// Runner
        /// </summary>
        /// <param name="runs">Runs</param>
        /// <param name="priority">Priority</param>
        /// <returns>Scalable</returns>
        IScalable Runner(IDynamicRuns runs, QueuePriority priority);

        /// <summary>
        /// Minimum Frequency
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Minimum Frequency</returns>
        byte MinimumFrequency(QueuePriority priority);

        /// <summary>
        /// Maximum Frequency
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>Maximum Frequency</returns>
        byte MaximumFrequency(QueuePriority priority);
        #endregion
    }
    #endregion
}
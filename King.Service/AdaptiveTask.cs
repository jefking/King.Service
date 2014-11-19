namespace King.Service
{
    using King.Service.Timing;

    /// <summary>
    /// Adaptive Task
    /// </summary>
    /// <remarks>
    /// A task that determines its frequency through work load
    /// </remarks>
    public abstract class AdaptiveTask : DynamicTask
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="minimumPeriodInSeconds">Minimum, time in seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum, time in seconds</param>
        public AdaptiveTask(int minimumPeriodInSeconds = BaseTimes.MinimumTiming, int maximumPeriodInSeconds = BaseTimes.MaximumTiming)
            : base(new AdaptiveTiming(minimumPeriodInSeconds, maximumPeriodInSeconds), minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}
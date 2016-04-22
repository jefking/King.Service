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
        /// <param name="strategy">Timing Strategy</param>
        public AdaptiveTask(int minimumPeriodInSeconds = BaseTimes.DefaultMinimumTiming, int maximumPeriodInSeconds = BaseTimes.DefaultMaximumTiming, Strategy strategy = Strategy.Exponential)
            : base(Strategies.Adaptive(strategy, minimumPeriodInSeconds, maximumPeriodInSeconds), minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}
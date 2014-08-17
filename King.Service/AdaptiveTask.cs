namespace King.Service
{
    using King.Service.Timing;

    /// <summary>
    /// A task that determines its frequency through actions taken
    /// </summary>
    /// <remarks>
    /// If there is work is to be done then the task gets more aggressive
    /// If there is no work to be done then the task backs off
    /// - The result should be that the task is working at a needed capacity.
    /// </remarks>
    public abstract class AdaptiveTask : DynamicTask
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="minimumPeriodInSeconds">Minimum, time in seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum, time in seconds</param>
        public AdaptiveTask(int minimumPeriodInSeconds = 45, int maximumPeriodInSeconds = 300)
            : base(new AdaptiveTiming(), minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}
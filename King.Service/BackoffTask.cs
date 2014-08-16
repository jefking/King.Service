namespace King.Service
{
    /// <summary>
    /// Exponential Backoff Task
    /// </summary>
    /// <remarks>
    /// When a run is made and no work done, the task backs off.
    /// - Perfect for dequeuing when there might not be work items in the queue.
    /// </remarks>
    public abstract class BackoffTask : DynamicTask
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="minimumPeriodInSeconds">Minimum, time in seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum, time in seconds</param>
        public BackoffTask(int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
            : base(new BackoffTiming(), minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}
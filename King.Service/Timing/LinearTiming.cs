namespace King.Service.Timing
{
    /// <summary>
    /// Linear Timing
    /// </summary>
    public class LinearTiming : CalculateTiming
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public LinearTiming(int minimumPeriodInSeconds = BaseTimes.MinimumTiming, int maximumPeriodInSeconds = BaseTimes.MaximumTiming)
            : base(minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Linear strategy
        /// </summary>
        /// <param name="attempts">attempts</param>
        /// <returns>timing</returns>
        public override double Get(ulong attempts)
        {
            return attempts > 10 ? this.periodInSeconds.Maximum : this.periodInSeconds.Minimum + (((this.periodInSeconds.Maximum - this.periodInSeconds.Minimum) * .1) * attempts);
        }
        #endregion
    }
}
namespace King.Service.Timing
{
    using System;

    /// <summary>
    /// Backoff Timing
    /// </summary>
    public class BackoffTiming : DynamicTiming
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="minimumPeriodInSeconds">Minimum Period in Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period in Seconds</param>
        public BackoffTiming(int minimumPeriodInSeconds, int maximumPeriodInSeconds)
            : base(new ExponentialTiming(minimumPeriodInSeconds, maximumPeriodInSeconds))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="timing">Calculate Timing</param>
        public BackoffTiming(ICalculateTiming timing)
            : base(timing)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get Timing
        /// </summary>
        /// <param name="workWasDone">Work was done</param>
        /// <returns>New Timing</returns>
        public override double Get(bool workWasDone)
        {
            this.noWorkCount = workWasDone ? 0 : this.noWorkCount + 1;

            return this.timing.Get(this.noWorkCount);
        }
        #endregion
    }
}
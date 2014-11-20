namespace King.Service.Timing
{
    using System;

    /// <summary>
    /// Adaptive Timing
    /// </summary>
    public class AdaptiveTiming : DynamicTiming
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="minimumPeriodInSeconds">Minimum Period in Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period in Seconds</param>
        public AdaptiveTiming(int minimumPeriodInSeconds = BaseTimes.MinimumTiming, int maximumPeriodInSeconds = BaseTimes.MaximumTiming)
            : base(new ExponentialTiming(minimumPeriodInSeconds, maximumPeriodInSeconds))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="timing">Calculate Timing</param>
        public AdaptiveTiming(ICalculateTiming timing)
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
            if (workWasDone)
            {
                if (0 < this.noWorkCount)
                {
                    this.noWorkCount--;
                }
            }
            else if (this.timing.MaximumPeriodInSeconds > this.timing.Get(this.noWorkCount))
            {
                this.noWorkCount++;
            }

            return this.timing.Get(this.noWorkCount);
        }
        #endregion
    }
}
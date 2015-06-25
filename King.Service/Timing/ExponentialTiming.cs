namespace King.Service.Timing
{
    using System;

    /// <summary>
    /// Exponential Timing
    /// </summary>
    public class ExponentialTiming : CalculateTiming
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public ExponentialTiming(int minimumPeriodInSeconds = BaseTimes.MinimumTiming, int maximumPeriodInSeconds = BaseTimes.MaximumTiming)
            :base(minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Exponential strategy, within bounds
        /// </summary>
        /// <param name="attempts">attempts</param>
        /// <returns>timing</returns>
        public override double Get(ulong attempts)
        {
            if (0 == attempts)
            {
                return this.periodInSeconds.Minimum;
            }

            var current = ((Math.Pow(2, attempts) * .1d) * this.periodInSeconds.Minimum) + this.periodInSeconds.Minimum;
            return current < this.periodInSeconds.Maximum ? current : this.periodInSeconds.Maximum;
        }
        #endregion
    }
}
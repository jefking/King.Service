namespace King.Service.Timing
{
    using System;

    /// <summary>
    /// Timing Maths
    /// </summary>
    public class CalculateTiming : ICalculateTiming
    {
        #region Members
        /// <summary>
        /// Minimum Timeframe (seconds) (starting timeframe)
        /// </summary>
        protected readonly int minimumPeriodInSeconds;

        /// <summary>
        /// Maximum Timeframe (seconds) to backoff too.
        /// </summary>
        protected readonly int maximumPeriodInSeconds;
        #endregion
        
        #region Constructors
        public CalculateTiming(int minimumPeriodInSeconds, int maximumPeriodInSeconds)
        {
            if (0 >= minimumPeriodInSeconds)
            {
                throw new ArgumentException("Minimum Period In Seconds must be greater than 0.");
            }
            if (minimumPeriodInSeconds >= maximumPeriodInSeconds)
            {
                throw new ArgumentException("Mminimum Period In Seconds must be less than Maximum Period In Seconds");
            }

            this.minimumPeriodInSeconds = minimumPeriodInSeconds;
            this.maximumPeriodInSeconds = maximumPeriodInSeconds;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Minimum Timeframe (seconds) (starting timeframe)
        /// </summary>
        public virtual int MinimumPeriodInSeconds
        {
            get
            {
                return this.minimumPeriodInSeconds;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Exponential Backoff strategy, within bounds
        /// </summary>
        /// <param name="attempts">attempts</param>
        /// <param name="max">upper bound</param>
        /// <param name="min">lower bound</param>
        /// <returns>timing</returns>
        public virtual double Exponential(ulong attempts)
        {
            if (0 == attempts)
            {
                return this.minimumPeriodInSeconds;
            }

            var current = ((Math.Pow(2, attempts) * .1d) * this.minimumPeriodInSeconds) + this.minimumPeriodInSeconds;
            return current < this.maximumPeriodInSeconds ? current : this.maximumPeriodInSeconds;
        }
        #endregion
    }
}
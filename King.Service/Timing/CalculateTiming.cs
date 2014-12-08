namespace King.Service.Timing
{
    using System;

    /// <summary>
    /// Timing Maths
    /// </summary>
    public abstract class CalculateTiming : ICalculateTiming
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
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public CalculateTiming(int minimumPeriodInSeconds, int maximumPeriodInSeconds)
        {
            this.minimumPeriodInSeconds = 0 >= minimumPeriodInSeconds ? 1 : minimumPeriodInSeconds;
            this.maximumPeriodInSeconds = minimumPeriodInSeconds >= maximumPeriodInSeconds ? minimumPeriodInSeconds + 1 : maximumPeriodInSeconds;
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

        /// <summary>
        /// Maximum Timeframe (seconds)
        /// </summary>
        public virtual int MaximumPeriodInSeconds
        {
            get
            {
                return this.maximumPeriodInSeconds;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get timing
        /// </summary>
        /// <param name="attempts">attempts</param>
        /// <returns>timing</returns>
        public abstract double Get(ulong attempts);
        #endregion
    }
}
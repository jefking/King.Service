namespace King.Service.Timing
{
    using System;

    /// <summary>
    /// Calculate Timing
    /// </summary>
    public abstract class CalculateTiming : ICalculateTiming
    {
        #region Members
        /// <summary>
        /// Timeframe (seconds) (starting timeframe)
        /// </summary>
        protected readonly Range<int> periodInSeconds;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        public CalculateTiming(int minimumPeriodInSeconds, int maximumPeriodInSeconds)
        {
            this.periodInSeconds = new Range<int>
            {
                Minimum = 0 >= minimumPeriodInSeconds ? 1 : minimumPeriodInSeconds,
                Maximum = minimumPeriodInSeconds >= maximumPeriodInSeconds ? minimumPeriodInSeconds + 1 : maximumPeriodInSeconds,
            };
        }
        #endregion

        #region Properties
        /// Timeframe (seconds) (starting timeframe)
        /// </summary>
        public virtual Range<int> FrequencyInSeconds
        {
            get
            {
                return this.periodInSeconds;
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
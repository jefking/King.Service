namespace King.Service.Timing
{
    using System;

    /// <summary>
    /// Backoff Timing
    /// </summary>
    public class BackoffTiming : IDynamicTiming
    {
        #region Members
        /// <summary>
        /// Attempts Made
        /// </summary>
        private volatile ulong noWorkCount = 0;

        /// <summary>
        /// Timing Halper
        /// </summary>
        protected readonly ICalculateTiming timing = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="minimumPeriodInSeconds">Minimum Period in Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period in Seconds</param>
        public BackoffTiming(int minimumPeriodInSeconds, int maximumPeriodInSeconds)
            :this(new CalculateTiming(minimumPeriodInSeconds, maximumPeriodInSeconds))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="timing">Calculate Timing</param>
        public BackoffTiming(ICalculateTiming timing)
        {
            if (null == timing)
            {
                throw new ArgumentNullException("timing");
            }

            this.timing = timing;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get Timing
        /// </summary>
        /// <param name="workWasDone">Work was done</param>
        /// <returns>New Timing</returns>
        public virtual double Get(bool workWasDone)
        {
            this.noWorkCount = workWasDone ? 0 : this.noWorkCount + 1;

            return this.timing.Exponential(this.noWorkCount);
        }
        #endregion
    }
}
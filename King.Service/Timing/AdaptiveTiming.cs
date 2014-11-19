namespace King.Service.Timing
{
    using System;

    /// <summary>
    /// Adaptive Timing
    /// </summary>
    public class AdaptiveTiming : IDynamicTiming
    {
        #region Members
        /// <summary>
        /// Attempts Made
        /// </summary>
        private ulong noWorkCount = 0;

        /// <summary>
        /// Timing Helper
        /// </summary>
        protected readonly ICalculateTiming timing = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="minimumPeriodInSeconds">Minimum Period in Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period in Seconds</param>
        public AdaptiveTiming(int minimumPeriodInSeconds, int maximumPeriodInSeconds)
            : this(new ExponentialTiming(minimumPeriodInSeconds, maximumPeriodInSeconds))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="timing">Calculate Timing</param>
        public AdaptiveTiming(ICalculateTiming timing)
        {
            if (null == timing)
            {
                throw new ArgumentNullException("timing");
            }

            this.timing = timing;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Timing Helper
        /// </summary>
        public virtual ICalculateTiming Timing
        {
            get
            {
                return this.timing;
            }
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
            if (workWasDone)
            {
                if (0 < this.noWorkCount)
                {
                    this.noWorkCount--;
                }
            }
            else
            {
                this.noWorkCount++;
            }

            return this.timing.Get(this.noWorkCount);
        }
        #endregion
    }
}
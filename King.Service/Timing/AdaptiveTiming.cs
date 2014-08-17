namespace King.Service.Timing
{
    using System;

    public class AdaptiveTiming : IDynamicTiming
    {
        #region Members
        /// <summary>
        /// Attempts Made
        /// </summary>
        private ulong noWorkCount = 0;

        /// <summary>
        /// Timing Halper
        /// </summary>
        protected readonly ICalculateTiming timing = null;
        #endregion

        #region Constructors
        public AdaptiveTiming(int minimumPeriodInSeconds, int maximumPeriodInSeconds)
            : this(new CalculateTiming(minimumPeriodInSeconds, maximumPeriodInSeconds))
        {
        }
        public AdaptiveTiming(ICalculateTiming timing)
        {
            if (null == timing)
            {
                throw new ArgumentNullException("timing");
            }

            this.timing = timing;
        }
        #endregion

        #region Methods
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

            return this.timing.Exponential(this.noWorkCount);
        }
        #endregion
    }
}
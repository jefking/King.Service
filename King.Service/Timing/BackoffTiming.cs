namespace King.Service.Timing
{
    using System;

    public class BackoffTiming : IDynamicTiming
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
        public BackoffTiming(int minimumPeriodInSeconds, int maximumPeriodInSeconds)
            :this(new CalculateTiming(minimumPeriodInSeconds, maximumPeriodInSeconds))
        {
        }

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
        public virtual double Get(bool workWasDone)
        {
            this.noWorkCount = workWasDone ? 0 : this.noWorkCount + 1;

            return this.timing.Exponential(this.noWorkCount);
        }
        #endregion
    }
}
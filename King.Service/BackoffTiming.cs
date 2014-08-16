namespace King.Service
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
        protected readonly ITiming timing = null;
        #endregion

        #region Constructors
        public BackoffTiming()
            :this(new Timing())
        { }

        public BackoffTiming(ITiming timing)
        {
            if (null == timing)
            {
                throw new ArgumentNullException("timing");
            }

            this.timing = timing;
        }
        #endregion

        #region Methods
        public virtual double Get(bool workWasDone, int max, int min = 1)
        {
            this.noWorkCount = workWasDone ? 0 : this.noWorkCount + 1;

            return this.timing.Exponential(this.noWorkCount, max, min);
        }
        #endregion
    }
}
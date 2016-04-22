namespace King.Service.Timing
{
    using System;

    /// <summary>
    /// Dynamic Timing
    /// </summary>
    public abstract class DynamicTiming : IDynamicTiming
    {
        #region Members
        /// <summary>
        /// Attempts Made
        /// </summary>
        protected ulong noWorkCount = 0;

        /// <summary>
        /// Timing
        /// </summary>
        protected readonly ICalculateTiming timing = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="timing">Calculate Timing</param>
        public DynamicTiming(ICalculateTiming timing)
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
        /// Timing
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
        public abstract double Get(bool workWasDone);
        #endregion
    }
}
namespace King.Service.Timing
{
    /// <summary>
    /// Timing interface for Timing Maths
    /// </summary>
    public interface ICalculateTiming
    {
        #region Properties
        /// <summary>
        /// Minimum Timeframe (seconds) (starting timeframe)
        /// </summary>
        int MinimumPeriodInSeconds
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get Timing
        /// </summary>
        /// <param name="attempts">Attempts</param>
        /// <returns>Timing</returns>
        double Get(ulong attempts);
        #endregion
    }

    /// <summary>
    /// Dynamic Timing
    /// </summary>
    public interface IDynamicTiming
    {
        #region Properties
        /// <summary>
        /// Timing Helper
        /// </summary>
        ICalculateTiming Timing
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get Timing
        /// </summary>
        /// <param name="workWasDone">Work Was Done</param>
        /// <returns>Timing</returns>
        double Get(bool workWasDone);
        #endregion
    }
}
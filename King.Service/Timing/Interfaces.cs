namespace King.Service.Timing
{
    /// <summary>
    /// Timing interface for Timing Maths
    /// </summary>
    public interface ICalculateTiming
    {
        #region Methods
        /// <summary>
        /// Exponential Timing
        /// </summary>
        /// <param name="attempts">Attempts</param>
        /// <returns>Timing</returns>
        double Exponential(ulong attempts);
        #endregion
    }

    /// <summary>
    /// Dynamic Timing
    /// </summary>
    public interface IDynamicTiming
    {
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
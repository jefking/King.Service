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
        /// <param name="max">Maximum Time Frame</param>
        /// <param name="min">Minimum Time Frame</param>
        /// <returns>Timing</returns>
        double Exponential(ulong attempts, int max, int min = 1);
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
        /// <param name="max">Maximum Time Frame</param>
        /// <param name="min">Minimum Time Frame</param>
        /// <returns>Timing</returns>
        double Get(bool workWasDone, int max, int min);
        #endregion
    }
}
namespace King.Service.Timing
{
    using System;

    #region ICalculateTiming
    /// <summary>
    /// Timing interface for Timing Maths
    /// </summary>
    public interface ICalculateTiming
    {
        #region Properties
        /// <summary>
        /// Timeframe (seconds) (starting timeframe)
        /// </summary>
        Range<int> FrequencyInSeconds
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
    #endregion

    #region IDynamicTiming
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
    #endregion

    #region ITimingTracker
    /// <summary>
    /// Timing Tracker Inteface
    /// </summary>
    public interface ITimingTracker
    {
        #region Methods
        /// <summary>
        /// Digest Duration
        /// </summary>
        /// <param name="duration">Duration</param>
        /// <returns>Size</returns>
        byte Calculate(TimeSpan duration, byte currentSize);
        #endregion
    }
    #endregion
}
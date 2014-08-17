namespace King.Service.Timing
{
    /// <summary>
    /// Timing interface for Timing Maths
    /// </summary>
    public interface ICalculateTiming
    {
        #region Methods
        double Exponential(ulong attempts, int max, int min = 1);
        #endregion
    }

    /// <summary>
    /// Dynamic Timing
    /// </summary>
    public interface IDynamicTiming
    {
        #region Methods
        double Get(bool workWasDone, int max, int min);
        #endregion
    }
}
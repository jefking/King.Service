namespace King.Service.Timing
{
    /// <summary>
    /// Timing Strategy
    /// </summary>
    public enum Strategy : byte
    {
        /// <summary>
        /// Exponential, Default
        /// </summary>
        Exponential = 0,

        /// <summary>
        /// Linear
        /// </summary>
        Linear = 1,
    }
}
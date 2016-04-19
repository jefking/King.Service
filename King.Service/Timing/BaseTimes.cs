namespace King.Service.Timing
{
    /// <summary>
    /// Base Times
    /// </summary>
    public struct BaseTimes
    {
        #region Members
        /// <summary>
        /// Minimum Storage Timing. (Seconds)
        /// </summary>
        public const byte MinimumStorageTiming = 10;

        /// <summary>
        /// Maximum Storage Timing. (Seconds)
        /// </summary>
        public const byte MaximumStorageTiming = 45;

        /// <summary>
        /// Slightly stager tasks, so they don't compete. (Milliseconds)
        /// </summary>
        public const byte ThreadingOffset = byte.MaxValue;

        /// <summary>
        /// Scale Check (Minutes)
        /// </summary>
        public const byte ScaleCheck = 2;
        #endregion
    }
}
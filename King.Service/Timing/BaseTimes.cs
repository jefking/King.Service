namespace King.Service.Timing
{
    /// <summary>
    /// Base Times
    /// </summary>
    public struct BaseTimes
    {
        #region Members
        /// <summary>
        /// Causes Threading Time to run Once
        /// </summary>
        public const int NoRepeat = -1;

        /// <summary>
        /// Initialization Timing
        /// </summary>
        public const int InitializationTiming = 10;

        /// <summary>
        /// Minimum Timing
        /// </summary>
        public const int MinimumTiming = 15;

        /// <summary>
        /// Maxium Timing
        /// </summary>
        public const int MaximumTiming = 120;

        /// <summary>
        /// Minimum Storage Timing
        /// </summary>
        public const int MinimumStorageTiming = 15;

        /// <summary>
        /// Maximum Storage Timing
        /// </summary>
        public const int MaximumStorageTiming = 180;

        /// <summary>
        /// Slightly stager tasks, so they don't compete.
        /// </summary>
        public const byte ThreadingOffset = byte.MaxValue;
        #endregion
    }
}
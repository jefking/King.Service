namespace King.Service.Timing
{
    /// <summary>
    /// Base Times
    /// </summary>
    public struct BaseTimes
    {
        #region Members
        /// <summary>
        /// Causes Threading Time to run Once. (Seconds)
        /// </summary>
        public const int NoRepeat = -1;

        /// <summary>
        /// Minimum Timing. (Seconds)
        /// </summary>
        public const int MinimumTiming = 10;

        /// <summary>
        /// Maxium Timing. (Seconds)
        /// </summary>
        public const int MaximumTiming = 180;

        /// <summary>
        /// Minimum Storage Timing. (Seconds)
        /// </summary>
        public const int MinimumStorageTiming = 15;

        /// <summary>
        /// Maximum Storage Timing. (Seconds)
        /// </summary>
        public const int MaximumStorageTiming = 180;

        /// <summary>
        /// Slightly stager tasks, so they don't compete. (Milliseconds)
        /// </summary>
        public const byte ThreadingOffset = byte.MaxValue;

        /// <summary>
        /// Scale Check (Minutes)
        /// </summary>
        public const byte ScaleCheck = 5;
        #endregion
    }
}
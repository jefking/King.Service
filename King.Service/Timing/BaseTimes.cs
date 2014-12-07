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
        public const sbyte NoRepeat = -1;

        /// <summary>
        /// Minimum Timing. (Seconds)
        /// </summary>
        public const byte MinimumTiming = 10;

        /// <summary>
        /// Maxium Timing. (Seconds)
        /// </summary>
        public const byte MaximumTiming = 180;

        /// <summary>
        /// Minimum Storage Timing. (Seconds)
        /// </summary>
        public const byte MinimumStorageTiming = 15;

        /// <summary>
        /// Maximum Storage Timing. (Seconds)
        /// </summary>
        public const byte MaximumStorageTiming = 180;

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
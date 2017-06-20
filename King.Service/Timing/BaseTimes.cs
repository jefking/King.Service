namespace King.Service.Timing
{
    /// <summary>
    /// Base Times
    /// </summary>
    public struct BaseTimes
    {
        #region Members
        /// <summary>
        /// Default Minimum Timing. (Seconds)
        /// </summary>
        public const byte DefaultMinimumTiming = 10;

        /// <summary>
        /// Default Maximum Timing. (Seconds)
        /// </summary>
        public const byte DefaultMaximumTiming = 45;

        /// <summary>
        /// Slightly stager tasks, so they don't compete. (Milliseconds)
        /// </summary>
        //public const byte ThreadingOffset = byte.MaxValue;

        /// <summary>
        /// Scale Check (Minutes)
        /// </summary>
        public const byte ScaleCheck = 2;
        #endregion
    }
}
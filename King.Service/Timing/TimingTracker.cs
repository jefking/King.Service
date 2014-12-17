namespace King.Service.Timing
{
    using King.Service.Data;
    using System;

    /// <summary>
    /// Timing Tracker
    /// </summary>
    public class TimingTracker : ITimingTracker
    {
        #region Members
        /// <summary>
        /// Max Time
        /// </summary>
        protected readonly TimeSpan maxTime = TimeSpan.FromSeconds(45);

        /// <summary>
        /// Maximum batch size
        /// </summary>
        protected readonly byte max = byte.MaxValue;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxTime">Maximum time until time-outs occur</param>
        /// <param name="max">Maximum batch size</param>
        public TimingTracker(TimeSpan maxTime, byte max = byte.MaxValue)
        {
            if (TimeSpan.Zero >= maxTime)
            {
                throw new ArgumentException("maxTime");
            }

            this.maxTime = TimeSpan.FromTicks((long)Math.Round(maxTime.Ticks * .75, 0));
            this.max = max;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Digest Duration
        /// </summary>
        /// <param name="duration">Duration</param>
        /// <returns>Size</returns>
        public virtual byte Calculate(TimeSpan duration, byte currentSize)
        {
            var result = this.maxTime > duration ? (byte)currentSize + 1 : (byte)currentSize - 1;

            if (this.max <= result)
            {
                return this.max;
            }
            else if (byte.MinValue >= result)
            {
                return DequeueBatch<object>.MinimumBatchSize;
            }
            else
            {
                return (byte)result;
            }
        }
        #endregion
    }
}
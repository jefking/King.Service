namespace King.Service.Timing
{
    using King.Service.Data;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

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
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxTime">Maximum time until time-outs occur</param>
        public TimingTracker(TimeSpan maxTime)
        {
            if (TimeSpan.Zero >= maxTime)
            {
                throw new ArgumentException("maxTime");
            }

            this.maxTime = maxTime;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Digest Duration
        /// </summary>
        /// <param name="duration">Duration</param>
        /// <returns>Size</returns>
        public byte Calculate(TimeSpan duration, byte currentSize)
        {
            var max = this.maxTime.Ticks * .7;
            var result = max > duration.Ticks ? (byte)currentSize + 1 : (byte)currentSize - 1;

            if (byte.MaxValue <= result)
            {
                return byte.MaxValue;
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
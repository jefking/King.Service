namespace King.Service.Timing
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Timing Tracker
    /// </summary>
    public class TimingTracker : ITimingTracker
    {
        #region Members
        /// <summary>
        /// Max Time
        /// </summary>
        private readonly TimeSpan maxTime;

        /// <summary>
        /// Times
        /// </summary>
        protected readonly Stack<TimeSpan> times = new Stack<TimeSpan>(byte.MaxValue);
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
        public void Digest(TimeSpan duration)
        {
            this.times.Push(duration);
        }

        /// <summary>
        /// Batch Size
        /// </summary>
        /// <returns>Size</returns>
        public byte BatchSize()
        {
            if (0 == this.times.Count)
            {
                return 1;
            }

            return 1;
        }
        #endregion
    }
}
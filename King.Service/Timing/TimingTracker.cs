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
        public void Digest(TimeSpan timespan)
        {
            this.times.Push(timespan);
        }

        public byte BatchSize()
        {
            return 1;
        }
        #endregion
    }
}
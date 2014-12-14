namespace King.Service.Timing
{
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
        protected readonly TimeSpan maxTime;

        /// <summary>
        /// Times
        /// </summary>
        protected readonly ConcurrentStack<TimeSpan> times = new ConcurrentStack<TimeSpan>();
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
                return 1;//should this be a no-op?
            }

            //take all
            var durations = new TimeSpan[this.times.Count];
            this.times.CopyTo(durations, 0);
            this.times.Clear();

            var average = durations.Average(d => d.Ticks);

            var result = this.maxTime.Ticks / average;

            return result <= byte.MinValue
                ? (byte)1
                : result > byte.MinValue
                ? byte.MaxValue
                : (byte)result;
        }
        #endregion
    }
}
namespace King.Service
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Exponential Backoff Task
    /// </summary>
    /// <remarks>
    /// When a run is made and no work done, the task backs off.
    /// - Perfect for dequeuing when there might not be work items in the queue.
    /// </remarks>
    public abstract class BackoffTask : TaskManager
    {
        #region Members
        /// <summary>
        /// Minimum Timeframe (seconds) (starting timeframe)
        /// </summary>
        protected readonly int minimumPeriodInSeconds = 60;

        /// <summary>
        /// Maximum Timeframe (seconds) to backoff too.
        /// </summary>
        protected readonly int maximumPeriodInSeconds = 300;

        /// <summary>
        /// Attempts Made
        /// </summary>
        private ulong noWorkCount = 0;

        /// <summary>
        /// Timing Halper
        /// </summary>
        protected readonly ITiming timing = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="minimumPeriodInSeconds">Minimum, time in seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum, time in seconds</param>
        public BackoffTask(int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
            : this(new Timing(), minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }

        /// <summary>
        /// Constructor for Mocking
        /// </summary>
        /// <param name="timing">Timing</param>
        /// <param name="minimumPeriodInSeconds">Minimum, time in seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum, time in seconds</param>
        public BackoffTask(ITiming timing, int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
            : base(minimumPeriodInSeconds, minimumPeriodInSeconds)
        {
            if (null == timing)
            {
                throw new ArgumentNullException("timing");
            }
            if (0 >= minimumPeriodInSeconds)
            {
                throw new ArgumentException("Minimum Period In Seconds must be greater than 0.");
            }
            if (minimumPeriodInSeconds >= maximumPeriodInSeconds)
            {
                throw new ArgumentException("Mminimum Period In Seconds must be less than Maximum Period In Seconds");
            }

            this.timing = timing;
            this.minimumPeriodInSeconds = minimumPeriodInSeconds;
            this.maximumPeriodInSeconds = maximumPeriodInSeconds;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run
        /// </summary>
        public override void Run()
        {
            bool workWasDone;
            this.Run(out workWasDone);

            Trace.TraceInformation("{0}: {1}Work done. (@ {2})", this.GetType().ToString(), workWasDone ? string.Empty : "No ", DateTime.UtcNow);

            this.noWorkCount = workWasDone ? 0 : this.noWorkCount + 1;

            var newTime = this.timing.Exponential(this.noWorkCount, this.maximumPeriodInSeconds, this.minimumPeriodInSeconds);

            if (base.Every.TotalSeconds != newTime)
            {
                var ts = TimeSpan.FromSeconds(newTime);
                
                base.ChangeTiming(ts);

                Trace.TraceInformation("{0}: New timing to: {1}. (@ {2})", this.GetType().ToString(), ts, DateTime.UtcNow);
            }
        }

        /// <summary>
        /// Run
        /// </summary>
        /// <param name="workWasDone">Work Was Done</param>
        public abstract void Run(out bool workWasDone);
        #endregion
    }
}
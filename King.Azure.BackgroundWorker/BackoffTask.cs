namespace King.Azure.BackgroundWorker
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
        private readonly int minimumPeriodInSeconds = 60;

        /// <summary>
        /// Maximum Timeframe (seconds) to backoff too.
        /// </summary>
        private readonly int maximumPeriodInSeconds = 300;

        /// <summary>
        /// Attempts Made
        /// </summary>
        private ulong noWorkCount = 0;

        /// <summary>
        /// Current Time
        /// </summary>
        private double currentTime = 0;

        /// <summary>
        /// Timing Halper
        /// </summary>
        private readonly ITiming timing = new Timing();
        #endregion

        #region Constructors
        public BackoffTask(int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
            : this(new Timing(), minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }

        /// <summary>
        /// Constructor for Mocking
        /// </summary>
        /// <param name="timing">Timing</param>
        public BackoffTask(ITiming timing, int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
            : base(minimumPeriodInSeconds, maximumPeriodInSeconds)
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
            this.currentTime = minimumPeriodInSeconds;
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

            this.noWorkCount = workWasDone ? 0 : this.noWorkCount + 1;

            var newTime = this.timing.Exponential(this.minimumPeriodInSeconds, this.maximumPeriodInSeconds, this.noWorkCount);

            if (currentTime != newTime)
            {
                var ts = TimeSpan.FromSeconds(newTime);
                
                base.ChangeTiming(ts);

                currentTime = ts.TotalSeconds;

                Trace.TraceInformation("Changed timing to: {0}.", ts);
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
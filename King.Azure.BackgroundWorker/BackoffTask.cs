namespace King.Azure.BackgroundWorker
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Exponential Backoff Task
    /// </summary>
    /// <remarks>
    /// When an attempt is made and no work done, the tasks can be backed off.
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
        private int attempts = 0;

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
            : base(minimumPeriodInSeconds, minimumPeriodInSeconds)
        {
            if (null == timing)
            {
                throw new ArgumentNullException("timing");
            }
            if (0 >= minimumPeriodInSeconds)
            {
                throw new ArgumentException("minimumPeriodInSeconds must be greater than 0.");
            }
            if (minimumPeriodInSeconds >= maximumPeriodInSeconds)
            {
                throw new ArgumentException("minimumPeriodInSeconds must be less than maximumPeriodInSeconds");
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

            var workCount = workWasDone ? 0 : this.attempts + 1;

            if (!workWasDone)
            {
                this.attempts++;

                var newTime = this.timing.Exponential(minimumPeriodInSeconds, maximumPeriodInSeconds, attempts);
                var ts = TimeSpan.FromSeconds(newTime);

                base.Change(ts);

                Trace.TraceInformation("Changed timing to: {0}.", ts);
            }

            this.attempts = workCount;
        }

        /// <summary>
        /// Run
        /// </summary>
        /// <param name="workWasDone">Work Was Done</param>
        public abstract void Run(out bool workWasDone);
        #endregion
    }
}
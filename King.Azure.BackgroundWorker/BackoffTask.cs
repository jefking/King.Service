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
        /// Minimum Timeframe (starting timeframe)
        /// </summary>
        private readonly int min = 60;

        /// <summary>
        /// Maximum Timeframe to backoff too.
        /// </summary>
        private readonly int max = 300;

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
        public BackoffTask(int min = 60, int max = 300)
            : this(new Timing(), min, max)
        {
        }

        /// <summary>
        /// Constructor for Mocking
        /// </summary>
        /// <param name="timing">Timing</param>
        public BackoffTask(ITiming timing, int min = 60, int max = 300)
            : base(min, min)
        {
            if (0 == min)
            {

            }
            if (min >= max)
            {

            }
            if (null == timing)
            {

            }

            this.timing = timing;
        }
        #endregion

        #region Methods
        public override void Run()
        {
            bool workDone;
            this.Run(out workDone);

            if (workDone && this.attempts > 0)//wromg
            {
                this.attempts++;

                var newTime = this.timing.Exponential(min, max, attempts);
                var ts = TimeSpan.FromSeconds(newTime);

                base.Change(ts);

                Trace.TraceInformation("Changed time to: {0}.", ts);
            }
        }

        public abstract void Run(out bool workDone);
        #endregion
    }
}
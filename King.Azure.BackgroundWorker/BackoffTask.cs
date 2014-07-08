namespace King.Azure.BackgroundWorker
{
    using System;

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
        private readonly int min = 15;

        /// <summary>
        /// Maximum Timeframe to backoff too.
        /// </summary>
        private readonly int max = 60;

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
        #endregion

        #region Methods
        public override void Run()
        {
            bool workDone;
            this.Run(out workDone);

            this.attempts = workDone ? 0 : this.attempts++;

            var newTime = this.timing.Exponential(min, max, attempts);
            var ts = TimeSpan.FromSeconds(newTime);

            base.Change(ts);
        }

        public abstract void Run(out bool workDone);
        #endregion
    }
}
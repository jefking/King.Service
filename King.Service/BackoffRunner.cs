namespace King.Service
{
    /// <summary>
    /// Simplified Backoff Runner
    /// </summary>
    public class BackoffRunner : BackoffTask
    {
        #region Members
        /// <summary>
        /// Backoff Runs
        /// </summary>
        protected readonly IBackoffRuns run = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="run">Run</param>
        public BackoffRunner(IBackoffRuns run)
            :base(run.MinimumPeriodInSeconds, run.MaximumPeriodInSeconds)
        {
            this.run = run;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run
        /// </summary>
        /// <param name="workWasDone">Work Was Done</param>
        public override void Run(out bool workWasDone)
        {
            workWasDone = this.run.Run().Result;
        }
        #endregion
    }
}
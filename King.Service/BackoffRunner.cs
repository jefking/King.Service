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
        protected readonly IDynamicRuns run = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="run">Run</param>
        public BackoffRunner(IDynamicRuns run)
            : base(run.MinimumPeriodInSeconds, run.MaximumPeriodInSeconds)
        {
            this.run = run;
            base.ServiceName = string.Format("{0}+{1}", this.GetType(), this.timing.GetType());
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
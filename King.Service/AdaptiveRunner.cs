namespace King.Service
{
    /// <summary>
    /// Simplified Adaptive Runner
    /// </summary>
    public class AdaptiveRunner : AdaptiveTask
    {
        #region Members
        /// <summary>
        /// Dynamic Runs
        /// </summary>
        protected readonly IDynamicRuns run = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="run">Run</param>
        public AdaptiveRunner(IDynamicRuns run)
            : base(run.MinimumPeriodInSeconds, run.MaximumPeriodInSeconds)
        {
            this.run = run;

            base.ServiceName = string.Format("{0}+{1}", this.GetType(), this.run.GetType());
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

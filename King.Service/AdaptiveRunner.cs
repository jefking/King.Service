namespace King.Service
{
    /// <summary>
    /// Simplified Adaptive Runner
    /// </summary>
    public class AdaptiveRunner : AdaptiveTask, IScalable
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

        #region Properties
        /// <summary>
        /// Service Should Scale
        /// </summary>
        public virtual bool Scale
        {
            get
            {
                return this.run.MinimumPeriodInSeconds == base.Every.TotalSeconds;
            }
        }
        #endregion
    }
}

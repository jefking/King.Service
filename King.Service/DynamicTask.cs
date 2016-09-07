namespace King.Service
{
    using King.Service.Timing;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Dynamic Task, base class for Tasks which change their own timing based on quantity of work
    /// </summary>
    public abstract class DynamicTask : RecurringTask, IScalable, IDisposable
    {
        #region Members
        /// <summary>
        /// Timing
        /// </summary>
        protected readonly IDynamicTiming timing = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for Mocking
        /// </summary>
        /// <param name="timing">Timing</param>
        public DynamicTask(IDynamicTiming timing)
            : base(timing.Timing.FrequencyInSeconds.Minimum)
        {
            if (null == timing)
            {
                throw new ArgumentNullException("timing");
            }

            this.timing = timing;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Task Should Scale
        /// </summary>
        public virtual bool Scale
        {
            get
            {
                return this.timing.Timing.FrequencyInSeconds.Minimum * 1000 == base.frequency.TotalMilliseconds;
            }
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

            Trace.TraceInformation("{0}: {1}Work done.", base.Name, workWasDone ? string.Empty : "No ");

            var newTime = this.timing.Get(workWasDone);

            var ts = TimeSpan.FromSeconds(newTime);

            if (base.frequency.TotalMilliseconds != ts.TotalMilliseconds)
            {
                base.Change(ts);

                Trace.TraceInformation("{0}: Changed timing to: {1}.", base.Name, ts);
            }
        }

        /// <summary>
        /// Run
        /// </summary>
        /// <param name="workWasDone">Work Was Done</param>
        public abstract void Run(out bool workWasDone);

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">Dispposing</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #endregion
    }
}
namespace King.Service
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class DynamicTask : TaskManager
    {
        #region Members
        /// <summary>
        /// Minimum Timeframe (seconds) (starting timeframe)
        /// </summary>
        protected readonly int minimumPeriodInSeconds;

        /// <summary>
        /// Maximum Timeframe (seconds) to backoff too.
        /// </summary>
        protected readonly int maximumPeriodInSeconds;

        /// <summary>
        /// Timing Helper
        /// </summary>
        protected readonly IDynamicTiming timing = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for Mocking
        /// </summary>
        /// <param name="timing">Timing</param>
        /// <param name="minimumPeriodInSeconds">Minimum, time in seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum, time in seconds</param>
        public DynamicTask(IDynamicTiming timing, int minimumPeriodInSeconds = 45, int maximumPeriodInSeconds = 300)
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

            var newTime = this.timing.Get(workWasDone, this.maximumPeriodInSeconds, this.minimumPeriodInSeconds);

            if (base.Every.TotalSeconds != newTime)
            {
                var ts = TimeSpan.FromSeconds(newTime);
                
                base.ChangeTiming(ts);

                Trace.TraceInformation("{0}: Changed timing to: {1}. (@ {2})", this.GetType().ToString(), ts, DateTime.UtcNow);
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
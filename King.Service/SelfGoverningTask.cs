namespace King.Service
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A task that determines its frequency through actions taken
    /// </summary>
    /// <remarks>
    /// If there is work is to be done then the task gets more aggressive
    /// If there is no work to be done then the task backs off
    /// - The result should be that the task is working at a needed capacity.
    /// </remarks>
    public class SelfGoverningTask : TaskManager
    {
        #region Members
        /// <summary>
        /// Minimum Timeframe (seconds) (starting timeframe)
        /// </summary>
        private readonly int minimumPeriodInSeconds = 30;

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
        private readonly ITiming timing = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="minimumPeriodInSeconds">Minimum, time in seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum, time in seconds</param>
        public SelfGoverningTask(int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
            : this(new Timing(), minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }

        /// <summary>
        /// Constructor for Mocking
        /// </summary>
        /// <param name="timing">Timing</param>
        /// <param name="minimumPeriodInSeconds">Minimum, time in seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum, time in seconds</param>
        public SelfGoverningTask(ITiming timing, int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
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

            Trace.TraceInformation("{0}: Work was {1}done. (Time: {2})", this.GetType().ToString(), workWasDone ? string.Empty : "not ", DateTime.UtcNow);

            this.noWorkCount = workWasDone ? 0 : this.noWorkCount + 1;

            var newTime = this.timing.Exponential(this.noWorkCount, this.maximumPeriodInSeconds, this.minimumPeriodInSeconds);

            if (currentTime != newTime)
            {
                var ts = TimeSpan.FromSeconds(newTime);
                
                base.ChangeTiming(ts);

                currentTime = ts.TotalSeconds;

                Trace.TraceInformation("{0}: Changed timing to: {1}. (Time: {2})", this.GetType().ToString(), ts, DateTime.UtcNow);
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
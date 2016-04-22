namespace King.Service
{
    using King.Service.Timing;
    using System;
    using System.Diagnostics;
    using System.Timers;

    /// <summary>
    /// Recurring Task
    /// </summary>
    public abstract class RecurringTask : IRunnable
    {
        #region Members
        /// <summary>
        /// Timer
        /// </summary>
        protected Timer timer = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        protected RecurringTask(int frequency = BaseTimes.DefaultMaximumTiming)
        {
            if (0 >= frequency)
            {
                throw new ArgumentException("Frequency must be greater than 0.");
            }

            var ts = TimeSpan.FromSeconds(frequency);
            this.timer = new Timer(ts.TotalMilliseconds)
            {
                AutoReset = true,
                Enabled = false,
            };
            this.timer.Elapsed += this.Run;

            this.Name = this.GetType().ToString();

            Trace.TraceInformation("{0} Frequency is: {1}s.", this.Name, ts);
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~RecurringTask()
        {
            Dispose(false);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Start Task
        /// </summary>
        /// <returns>Running</returns>
        public virtual bool Start()
        {
            this.timer.Start();

            return true;
        }

        /// <summary>
        /// Stop Task
        /// </summary>
        /// <returns>Stopped</returns>
        public virtual bool Stop()
        {
            this.timer.Stop();

            return true;
        }

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="state">State of Timer</param>
        /// <param name="e">Elapsed Event Args</param>
        public virtual void Run(object state, ElapsedEventArgs e)
        {
            var signal = null == e ? DateTime.UtcNow : e.SignalTime;

            var timing = Stopwatch.StartNew();

            try
            {
                this.Run();
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0}: {1}", this.Name, ex.ToString());
            }
            finally
            {
                timing.Stop();
            }

            Trace.TraceInformation("{0}: Task Completed, Duration: {1}, Signal Time: {2}", this.Name, timing.Elapsed, signal);
        }

        /// <summary>
        /// Change frequency to new interval
        /// </summary>
        /// <param name="frequency">Frequency</param>
        protected virtual void Change(TimeSpan frequency)
        {
            if (TimeSpan.Zero >= frequency)
            {
                throw new ArgumentException("frequency less than or equal to zero");
            }

            this.timer.Interval = frequency.TotalMilliseconds;
        }

        /// <summary>
        /// Run
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">Disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != timer)
                {
                    this.timer.Dispose();
                    this.timer = null;
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Name (For Tracing)
        /// </summary>
        protected virtual string Name
        {
            get;
            set;
        }
        #endregion
    }
}
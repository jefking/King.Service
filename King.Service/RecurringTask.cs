namespace King.Service
{
    using King.Service.Timing;
    using System;
    using System.Diagnostics;
    using System.Threading;

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

        /// <summary>
        /// Frequency
        /// </summary>
        protected TimeSpan frequency;
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

            this.frequency = TimeSpan.FromSeconds(frequency);
            this.timer = new Timer(this.Run, this, Timeout.Infinite, Timeout.Infinite);

            this.Name = this.GetType().ToString();

            Trace.TraceInformation("{0} Frequency is: {1}s.", this.Name, this.frequency);
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
            return timer.Change(TimeSpan.FromMilliseconds(1), frequency);
        }

        /// <summary>
        /// Stop Task
        /// </summary>
        /// <returns>Stopped</returns>
        public virtual bool Stop()
        {
            return this.timer.Change(0, 0);
        }

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="state">State of Timer</param>
        public virtual void Run(object state)
        {
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

            Trace.TraceInformation("{0}: Task Completed, Duration: {1}", this.Name, timing.Elapsed);
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

            this.frequency = frequency;

            this.timer.Change(TimeSpan.FromMilliseconds(1), this.frequency);
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
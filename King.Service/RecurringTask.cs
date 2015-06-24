namespace King.Service
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using King.Service.Timing;

    /// <summary>
    /// Recurring Task
    /// </summary>
    public abstract class RecurringTask : IRunnable
    {
        #region Members
        /// <summary>
        /// Timer
        /// </summary>
        private Timer timer = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected RecurringTask(int dueInSeconds = BaseTimes.MinimumTiming, int periodInSeconds = BaseTimes.MaximumTiming)
        {
            dueInSeconds = 0 > dueInSeconds ? BaseTimes.MinimumTiming : dueInSeconds;

            this.StartIn = TimeSpan.FromSeconds(dueInSeconds);
            this.Frequency  = 0 > periodInSeconds ? TimeSpan.Zero : TimeSpan.FromSeconds(periodInSeconds);
            this.Name = this.GetType().ToString();

            Trace.TraceInformation("{0} is due: {1}s; Period: {2}s.", this.Name, dueInSeconds, periodInSeconds);
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
        /// Runs Service
        /// </summary>
        /// <returns>Running</returns>
        public virtual bool Start()
        {
            if (this.Stop())
            {
                this.timer = (TimeSpan.Zero == this.Frequency )
                    ? new Timer(this.Run, null, (int)this.StartIn.TotalSeconds, Timeout.Infinite)
                    : new Timer(this.Run, null, this.StartIn, this.Frequency );
            }

            return true;
        }

        /// <summary>
        /// Stops Service
        /// </summary>
        /// <returns>Stopped</returns>
        public virtual bool Stop()
        {
            if (null != this.timer)
            {
                this.timer.Dispose();
                this.timer = null;
            }

            return true;
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

            Trace.TraceInformation("{0}: Task Completed (Duration: {1}).", this.Name, timing.Elapsed);
        }

        /// <summary>
        /// Change Timer to new interval
        /// </summary>
        /// <param name="newTime">New Time</param>
        protected virtual void ChangeTiming(TimeSpan newTime)
        {
            if (TimeSpan.Zero == newTime)
            {
                throw new ArgumentException("newTime Zero.");
            }

            this.StartIn = this.Frequency  = newTime;

            this.Start();
        }

        /// <summary>
        /// Execute Action
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
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
                this.Stop();
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

        /// <summary>
        /// Due Time of Timer
        /// </summary>
        public virtual TimeSpan StartIn
        {
            get;
            private set;
        }

        /// <summary>
        /// Frequency
        /// </summary>
        public virtual TimeSpan Frequency 
        {
            get;
            private set;
        }
        #endregion
    }
}
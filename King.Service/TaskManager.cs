namespace King.Service
{
    using King.Service.Timing;
    using System;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// Service Manager
    /// </summary>
    public abstract class TaskManager : IRunnable, IDisposable
    {
        #region Members
        /// <summary>
        /// Timer
        /// </summary>
        private Timer timer = null;

        /// <summary>
        /// Service Name
        /// </summary>
        protected readonly string serviceName;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        protected TaskManager(int dueInSeconds = BaseTimes.MinimumTiming, int periodInSeconds = BaseTimes.MaximumTiming)
        {
            if (0 > dueInSeconds)
            {
                throw new ArgumentException("dueInSeconds");
            }

            this.StartIn = TimeSpan.FromSeconds(dueInSeconds);
            this.Every = 0 > periodInSeconds ? TimeSpan.Zero : TimeSpan.FromSeconds(periodInSeconds);
            this.serviceName = this.GetType().ToString();

            Trace.TraceInformation("{0} is due: {1}s; Period: {2}s.", this.serviceName, dueInSeconds, periodInSeconds);
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~TaskManager() 
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
                if (TimeSpan.Zero == this.Every)
                {
                    this.timer = new Timer(this.Run, null, (int)this.StartIn.TotalSeconds, Timeout.Infinite);
                }
                else
                {
                    this.timer = new Timer(this.Run, null, this.StartIn, this.Every);
                }
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
                Trace.TraceError("{0}: {1}", this.serviceName, ex.ToString());
            }
            finally
            {
                timing.Stop();
            }

            Trace.TraceInformation("{0}: Task Completed (Duration: {1}).", this.serviceName, timing.Elapsed);
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

            this.StartIn = this.Every = newTime;

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
                if (null != this.timer)
                {
                    this.timer.Dispose();
                    this.timer = null;
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Due Time of Timer
        /// </summary>
        public virtual TimeSpan StartIn
        {
            get;
            private set;
        }

        /// <summary>
        /// Period of Timer
        /// </summary>
        public virtual TimeSpan Every
        {
            get;
            private set;
        }
        #endregion
    }
}
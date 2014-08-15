namespace King.Service
{
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
        /// Due Time of Timer
        /// </summary>
        private TimeSpan due;

        /// <summary>
        /// Period of Timer
        /// </summary>
        private TimeSpan periodInSeconds;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        protected TaskManager(int dueInSeconds = 15, int periodInSeconds = 60)
        {
            if (0 > dueInSeconds)
            {
                throw new ArgumentException("dueInSeconds");
            }

            this.due = TimeSpan.FromSeconds(dueInSeconds);
            this.periodInSeconds = 0 > periodInSeconds ? TimeSpan.Zero : TimeSpan.FromSeconds(periodInSeconds);

            Trace.TraceInformation("{0} is due: {1}s; Period: {2}s.", this.GetType().ToString(), dueInSeconds, periodInSeconds);
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
                if (TimeSpan.Zero == this.periodInSeconds)
                {
                    this.timer = new Timer(this.Run, this, (int)this.due.TotalSeconds, Timeout.Infinite);
                }
                else
                {
                    this.timer = new Timer(this.Run, this, this.due, this.periodInSeconds);

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
                Trace.TraceError("{0}", ex.Message);
            }
            finally
            {
                timing.Stop();
            }

            Trace.TraceInformation("{0}: Task Completed (Duration: {1}).", this.GetType().ToString(), timing.Elapsed);
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

            this.due =
                this.periodInSeconds = newTime;

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
    }
}
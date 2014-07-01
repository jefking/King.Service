namespace King.Azure.BackgroundWorker
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
        private readonly TimeSpan dueTime;

        /// <summary>
        /// Period of Timer
        /// </summary>
        private readonly TimeSpan period;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        protected TaskManager(int dueInSeconds = 15, double periodInSeconds = 60)
        {
            if (0 > dueInSeconds)
            {
                throw new ArgumentException("dueInSeconds");
            }

            if (0 > periodInSeconds)
            {
                throw new ArgumentException("periodInSeconds");
            }

            this.dueTime = TimeSpan.FromSeconds(dueInSeconds);
            this.period = TimeSpan.FromSeconds(periodInSeconds);

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
        public bool Start()
        {
            this.timer = new Timer(this.Run, null, dueTime, period);

            return true;
        }

        /// <summary>
        /// Stops Service
        /// </summary>
        /// <returns>Stopped</returns>
        public bool Stop()
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
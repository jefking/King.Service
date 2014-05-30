namespace King.Service
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// Service Manager
    /// </summary>
    public abstract class Manager : IRunnable, IDisposable
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

        /// <summary>
        /// Disposed
        /// </summary>
        private volatile bool disposed = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        protected Manager(int dueInSeconds, double periodInSeconds)
        {
            if (0 > dueInSeconds)
            {
                throw new ArgumentException("dueInSeconds");
            }

            if (0 > periodInSeconds)
            {
                throw new ArgumentException("periodInSeconds");
            }

            Trace.WriteLine(string.Format("{0} is due in seconds: {1}; Period in seconds: {2}.", this.GetType(), dueInSeconds, periodInSeconds));

            this.dueTime = TimeSpan.FromSeconds(dueInSeconds);
            this.period = TimeSpan.FromSeconds(periodInSeconds);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Runs Service
        /// </summary>
        /// <returns>Running</returns>
        public bool Run()
        {
            this.timer = new Timer(this.Execute, null, dueTime, period);

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
        protected virtual void Execute(object state)
        {
            try
            {
                this.Execute();
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("{0}", ex.Message));
            }
        }

        /// <summary>
        /// Execute Action
        /// </summary>
        public abstract void Execute();

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
            if (!this.disposed)
            {
                if (null != this.timer)
                {
                    this.timer.Dispose();
                }

                this.timer = null;
                this.disposed = true;
            }
        }
        #endregion
    }
}
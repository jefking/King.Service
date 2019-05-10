namespace King.Service
{
    using King.Service.Timing;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Task Starter
    /// </summary>
    public class Starter : IStarter
    {
        #region Members
        /// <summary>
        /// Task Starter
        /// </summary>
        protected Timer starter = null;
        #endregion

        #region Methods
        /// <summary>
        /// Start
        /// </summary>
        /// <param name="tasks">Tasks</param>
        public void Start(IEnumerable<IRunnable> tasks)
        {
            if (null == tasks || 0 == tasks.Count())
            {
                return;
            }

            if (null == this.starter)
            {
                this.starter = new Timer(StartTask
                                        , new Stack<IRunnable>(tasks.Reverse())
                                        , BaseTimes.ThreadingOffset
                                        , BaseTimes.ThreadingOffset);
            }
        }

        /// <summary>
        /// Start Task
        /// </summary>
        /// <param name="state">Remaining Stack</param>
        public void StartTask(object state)
        {
            var stack = state as Stack<IRunnable>;
            if (null != stack && 0 < stack.Count)
            {
                var task = stack.Pop();
                if (task != null)
                {
                    try
                    {
                        var success = task.Start();

                        Trace.TraceInformation("{0} started: {1}.", task.GetType().ToString(), success);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Failed to start {0}: {1}", task.GetType().ToString(), ex.ToString());
                    }
                }
            }
        }

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
                if (null != this.starter)
                {
                    this.starter.Dispose();
                    this.starter = null;
                }
            }
        }
        #endregion
    }
}
namespace King.Service
{
    using King.Service.Timing;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Worker Role Service Manager
    /// </summary>
    public class RoleTaskManager<T> : IRoleTaskManager<T>
    {
        #region Members
        /// <summary>
        /// Services
        /// </summary>
        protected IReadOnlyCollection<IRunnable> tasks = null;

        /// <summary>
        /// Manager
        /// </summary>
        protected readonly ITaskFactory<T> manager = null;

        /// <summary>
        /// Lock object for Services
        /// </summary>
        protected readonly object servicesLock = new object();
        #endregion

        #region Constructors
        /// <summary>
        /// Pass Service Factory to run services
        /// </summary>
        /// <param name="services">Services</param>
        public RoleTaskManager(ITaskFactory<T> manager)
        {
            if (null == manager)
            {
                throw new ArgumentNullException("manager");
            }

            this.manager = manager;
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~RoleTaskManager() 
        {
            Dispose(false);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Tasks
        /// </summary>
        public IReadOnlyCollection<IRunnable> Tasks
        {
            get
            {
                return this.tasks;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run
        /// </summary>
        public virtual void Run()
        {
            Trace.TraceInformation("Run called");

            if (null != tasks && tasks.Any())
            {
                var serviceCount = tasks.Count();
                Trace.TraceInformation("Starting {0} services.", serviceCount);

                var successCount = 0;

                foreach (var s in tasks)
                {
                    try
                    {
                        s.Start();
                        
                        successCount++;

                        Trace.TraceInformation("{0} Started.", s.GetType().ToString());
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("{0}: {1}", s.GetType().ToString(), ex.ToString());
                    }

                    Thread.Sleep(BaseTimes.ThreadingOffset);
                }

                Trace.TraceInformation("Finished starting services {0}/{1} successfully.", successCount, serviceCount);
            }
            else
            {
                Trace.TraceWarning("No services to start up.");
            }

            Trace.TraceInformation("Run finished");
        }

        /// <summary>
        /// On Start
        /// </summary>
        /// <param name="passthrough">Passthrough</param>
        /// <returns>Started</returns>
        public virtual bool OnStart(T passthrough = default(T))
        {
            Trace.TraceInformation("On start called.");

            lock (this.servicesLock)
            {
                if (null == this.tasks)
                {
                    var t = this.manager.Tasks(passthrough);
                    if (null != t)
                    {
                        Trace.TraceInformation("Loading Services.");

                        this.tasks = new ReadOnlyCollection<IRunnable>(t.ToList());

                        Trace.TraceInformation("Services Loaded.");
                    }
                    else
                    {
                        Trace.TraceWarning("No Services loaded from Factory?");
                    }
                }
                else
                {
                    Trace.TraceInformation("Tasks already loaded.");
                }
            }

            Trace.TraceInformation("On start finished.");

            return true;
        }

        /// <summary>
        /// On Stop
        /// </summary>
        public virtual void OnStop()
        {
            Trace.TraceInformation("On stop called.");

            if (null != tasks && tasks.Any())
            {
                Trace.TraceInformation("Stopping {0} services.", tasks.Count());

                Parallel.ForEach(tasks, s =>
                {
                    try
                    {
                        var success = s.Stop();

                        Trace.TraceInformation("{0} Stopped; Success: {1}.", s.GetType().ToString(), success);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("{0}: {1}.", s.GetType().ToString(), ex.ToString());
                    }
                }
                );

                Trace.TraceInformation("Stopped {0} services.", tasks.Count());

                tasks = null;
            }
            else
            {
                Trace.TraceInformation("No services to stopped.");
            }

            Trace.TraceInformation("On stop finished.");
        }

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
                if (null != this.tasks)
                {
                    foreach (var t in tasks)
                    {
                        t.Dispose();
                    }
                }
            }
        }
        #endregion
    }
}
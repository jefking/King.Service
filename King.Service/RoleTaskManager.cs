namespace King.Service
{
    using King.Service.Timing;
    using System;
    using System.Collections.Generic;
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
        protected IEnumerable<IRunnable> services = null;

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
        #endregion

        #region Methods
        /// <summary>
        /// Run
        /// </summary>
        public virtual void Run()
        {
            Trace.TraceInformation("Run called");

            if (null != services && services.Any())
            {
                var serviceCount = services.Count();
                Trace.TraceInformation("Starting {0} services.", serviceCount);

                var successCount = 0;

                foreach (var s in services)
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
                if (null == this.services)
                {
                    Trace.TraceInformation("Loading Services.");

                    this.services = this.manager.Tasks(passthrough);

                    Trace.TraceInformation("Services Loaded.");
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

            if (null != services && services.Any())
            {
                Trace.TraceInformation("Stopping {0} services.", services.Count());

                Parallel.ForEach(services, s =>
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

                Trace.TraceInformation("Stopped {0} services.", services.Count());

                services = null;
            }
            else
            {
                Trace.TraceInformation("No services to stopped.");
            }

            Trace.TraceInformation("On stop finished.");
        }
        #endregion
    }
}
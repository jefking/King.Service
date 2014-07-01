namespace King.Azure.BackgroundWorker
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Worker Role Service Manager
    /// </summary>
    public class RoleServiceManager : IRoleServiceManager
    {
        #region Members
        /// <summary>
        /// Services
        /// </summary>
        private IEnumerable<IRunnable> services = null;

        /// <summary>
        /// Manager
        /// </summary>
        private readonly ITaskFactory<object> manager = null;

        /// <summary>
        /// Lock object for Mananger
        /// </summary>
        private readonly object managerLock = new object();
        #endregion

        #region Constructors
        /// <summary>
        /// Pass Service Factory to run services
        /// </summary>
        /// <param name="services">Services</param>
        public RoleServiceManager(ITaskFactory<object> manager)
        {
            if (null == manager)
            {
                throw new ArgumentNullException("manager");
            }

            lock (this.managerLock)
            {
                this.manager = manager;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run (Currently No-Op)
        /// </summary>
        public void Run()
        {
            Trace.TraceInformation("Run called");

            if (null != services && 0 < services.Count())
            {
                Trace.TraceInformation("Starting {0} services.", services.Count());

                Parallel.ForEach(services, s =>
                {
                    try
                    {
                       s.Start();

                        Trace.TraceInformation("{0} Started.", s.GetType().ToString());
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("{0}: {1}", s.GetType().ToString(), ex.Message);
                    }
                }
                );


                Trace.TraceInformation("Finished starting services.");
            }
            else
            {
                Trace.TraceInformation("No services to start up.");
            }

            Trace.TraceInformation("Run finished");
        }

        /// <summary>
        /// On Start
        /// </summary>
        /// <returns>Started</returns>
        public bool OnStart()
        {
            Trace.TraceInformation("On start called.");

            if (null == this.services)
            {
                Trace.TraceInformation("Loading Services.");

                lock (this.managerLock)
                {
                    this.services = manager.Tasks(this);
                }

                Trace.TraceInformation("Services Loaded.");
            }

            Trace.TraceInformation("On start finished.");

            return true;
        }

        /// <summary>
        /// On Stop
        /// </summary>
        public void OnStop()
        {
            Trace.TraceInformation("On stop called.");

            if (null != services && 0 < services.Count())
            {
                Trace.TraceInformation("Stopping {0} services.", services.Count());

                Parallel.ForEach(services, s =>
                {
                    try
                    {
                        var success = s.Stop();

                        Trace.TraceInformation("{0} Stopped, Success: {1}.", s.GetType().ToString(), success);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("{0}: {1}.", s.GetType().ToString(), ex.Message);
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
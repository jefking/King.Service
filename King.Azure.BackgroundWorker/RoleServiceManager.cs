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
    public class RoleServiceManager
    {
        #region Members
        /// <summary>
        /// Services
        /// </summary>
        private readonly IEnumerable<IRunnable> services;
        #endregion

        #region Constructors
        /// <summary>
        /// Pass Service Factory to run services
        /// </summary>
        /// <param name="services">Services</param>
        public RoleServiceManager(IServiceFactory<object> manager)
        {
            if (null == manager)
            {
                throw new ArgumentNullException("manager");
            }

            this.services = manager.Services(this);
        }
        #endregion

        #region Methods
        public void Run()
        {
            Trace.TraceInformation("Run called");

            Trace.TraceInformation("Run finished");
        }

        /// <summary>
        /// On Start
        /// </summary>
        /// <returns>Started</returns>
        public bool OnStart()
        {
            Trace.TraceInformation("On start called");

            bool totalSuccess = true;

            if (null != services && 0 < services.Count())
            {
                Trace.TraceInformation(string.Format("Starting {0} services.", services.Count()));

                Parallel.ForEach(services, s =>
                {
                    try
                    {
                        var success = s.Start();

                        Trace.WriteLine(string.Format("{0} Started: {1}", s.GetType(), success));

                        totalSuccess &= success;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(string.Format("{0}: {1}", s.GetType(), ex.Message));

                        totalSuccess = false;
                    }
                }
                );

                Trace.TraceInformation("Finished starting services");
            }
            else
            {
                Trace.TraceInformation("No services to start up.");
            }

            Trace.TraceInformation("On start finished");

            return totalSuccess;
        }

        /// <summary>
        /// On Stop
        /// </summary>
        public void OnStop()
        {
            Trace.TraceInformation("On stop called");

            if (null != services && 0 < services.Count())
            {
                Trace.TraceInformation(string.Format("Stopping {0} services.", services.Count()));

                Parallel.ForEach(services, s =>
                {
                    try
                    {
                        var success = s.Stop();

                        Trace.WriteLine(string.Format("{0} Stopped: {1}", s.GetType(), success));
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(string.Format("{0}: {1}", s.GetType(), ex.Message));
                    }
                }
                );

                Trace.TraceInformation("Finished stopping services");
            }
            else
            {
                Trace.TraceInformation("No services to stopped.");
            }

            Trace.TraceInformation("On stop finished");
        }
        #endregion
    }
}
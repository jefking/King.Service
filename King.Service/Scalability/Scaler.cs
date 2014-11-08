namespace King.Service.Scalability
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Task Scaler
    /// </summary>
    /// <typeparam name="T">Configuration</typeparam>
    public class Scaler<T> : IScaler<T>
    {
        #region Members
        /// <summary>
        /// Running Tasks
        /// </summary>
        protected readonly ConcurrentStack<IRoleTaskManager<T>> units = new ConcurrentStack<IRoleTaskManager<T>>();
        #endregion

        #region Properties
        /// <summary>
        /// Currnet Unit Count
        /// </summary>
        public virtual int CurrentUnits
        {
            get
            {
                return this.units.Count;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Should Scale
        /// </summary>
        /// <returns>Scale</returns>
        public virtual bool ShouldScale()
        {
            var timeToScale = 0;
            foreach (IScalable t in this.units.SelectMany(u => u.Tasks))
            {
                timeToScale += t.Scale ? 1 : -1;
            }

            return timeToScale != 0;
        }

        /// <summary>
        /// Is First Run
        /// </summary>
        /// <param name="minimum">Minimum</param>
        /// <returns>Is First Run</returns>
        public virtual bool IsFirstRun(byte minimum)
        {
            if (minimum < 1)
            {
                throw new ArgumentException("minimum");
            }

            return units.Count < minimum;
        }

        /// <summary>
        /// Initialize Scale
        /// </summary>
        /// <param name="minimum">Minimum</param>
        /// <param name="factory">Factory</param>
        /// <param name="serviceName">Service Name</param>
        public virtual void Initialize(byte minimum, ITaskFactory<T> factory, string serviceName)
        {
            if (minimum < 1)
            {
                throw new ArgumentException("minimum");
            }
            if (null == factory)
            {
                throw new ArgumentNullException("factory");
            }
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                throw new ArgumentException("serviceName");
            }

            while (units.Count < minimum)
            {
                this.ScaleUp(factory, serviceName);
            }
        }

        /// <summary>
        /// Scale Up by one unit
        /// </summary>
        public virtual void ScaleUp(ITaskFactory<T> factory, string serviceName)
        {
            if (null == factory)
            {
                throw new ArgumentNullException("factory");
            }
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                throw new ArgumentException("serviceName");
            }

            Trace.TraceInformation("Scaling Up: '{0}'.", serviceName);

            var unit = new RoleTaskManager<T>(factory);

            var success = unit.OnStart();
            if (success)
            {
                unit.Run();

                units.Push(unit);

                Trace.TraceInformation("Scaled Up: '{0}'.", serviceName);
            }
            else
            {
                unit.Dispose();

                Trace.TraceWarning("Failed to start Scale Unit: '{0}'.", serviceName);
            }
        }

        /// <summary>
        /// Scale down by one unit
        /// </summary>
        public virtual void ScaleDown(string serviceName)
        {
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                throw new ArgumentException("serviceName");
            }

            Trace.TraceInformation("Scaling Down: '{0}'.", serviceName);

            IRoleTaskManager<T> unit;
            if (units.TryPop(out unit))
            {
                unit.OnStop();
                unit.Dispose();
            }

            Trace.TraceInformation("Scaled Down: '{0}'.", serviceName);
        }
        #endregion
    }
}
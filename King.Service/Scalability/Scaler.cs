namespace King.Service.Scalability
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;

    /// <summary>
    /// Task Scaler
    /// </summary>
    /// <typeparam name="T">Configuration</typeparam>
    public class Scaler<T> : IScaler<T>
    {
        #region Methods
        /// <summary>
        /// Is First Run
        /// </summary>
        /// <param name="minimum">Minimum</param>
        /// <param name="units">Units</param>
        /// <returns>Is First Run</returns>
        public bool IsFirstRun(byte minimum, ConcurrentStack<IRoleTaskManager<T>> units)
        {
            return units.Count < minimum;
        }

        /// <summary>
        /// Initialize Scale
        /// </summary>
        /// <param name="minimum">Minimum</param>
        /// <param name="factory">Factory</param>
        /// <param name="units">Units</param>
        /// <param name="serviceName">Service Name</param>
        public virtual void Initialize(byte minimum, ITaskFactory<T> factory, ConcurrentStack<IRoleTaskManager<T>> units, string serviceName)
        {
            while (units.Count < minimum)
            {
                this.ScaleUp(factory, units, serviceName);
            }
        }

        /// <summary>
        /// Scale Up by one unit
        /// </summary>
        public virtual void ScaleUp(ITaskFactory<T> factory, ConcurrentStack<IRoleTaskManager<T>> units, string serviceName)
        {
            if (null == factory)
            {
                throw new ArgumentNullException("factory");
            }
            if (null == units)
            {
                throw new ArgumentNullException("units");
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
        public virtual void ScaleDown(ConcurrentStack<IRoleTaskManager<T>> units, string serviceName)
        {
            if (null == units)
            {
                throw new ArgumentNullException("units");
            }
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
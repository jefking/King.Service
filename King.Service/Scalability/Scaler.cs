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
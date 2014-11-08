﻿namespace King.Service
{
    using King.Service.Timing;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Task AutoScaler
    /// </summary>
    public abstract class AutoScaler<T> : RecurringTask, ITaskFactory<T>
    {
        #region Members
        /// <summary>
        /// Minimum Instance Count
        /// </summary>
        protected readonly byte minimum = 0;

        /// <summary>
        /// Maximum Instance Count
        /// </summary>
        protected readonly byte maximum = 1;

        /// <summary>
        /// Configuration
        /// </summary>
        protected readonly T configuration = default(T);

        /// <summary>
        /// Running Tasks
        /// </summary>
        protected readonly ConcurrentStack<IRoleTaskManager<T>> units = new ConcurrentStack<IRoleTaskManager<T>>();
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AutoScaler(T configuration = default(T), byte minimum = 0, byte maximum = 1)
            : base(BaseTimes.InitializationTiming, (int)TimeSpan.FromMinutes(15).TotalSeconds)
        {
            if (minimum > maximum)
            {
                throw new ArgumentException("Minimum should be less than Maximum");
            }

            this.configuration = configuration;
            this.minimum = minimum;
            this.maximum = maximum;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Minimum Instance Count
        /// </summary>
        public byte Minimum
        {
            get
            {
                return this.minimum;
            }
        }

        /// <summary>
        /// Maximum Instance Count
        /// </summary>
        public byte Maximum
        {
            get
            {
                return this.maximum;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Build Task Manifest
        /// </summary>
        /// <param name="passthrough">Configuration</param>
        /// <returns>Runnable Tasks</returns>
        public virtual IEnumerable<IRunnable> Tasks(T data)
        {
            return this.ScaleUnit(data);
        }

        /// <summary>
        /// Services which should be scaled as a group
        /// </summary>
        /// <param name="data">Configuration</param>
        /// <returns>Scalable Tasks</returns>
        public abstract IEnumerable<IScalable> ScaleUnit(T data);

        /// <summary>
        /// Task Run
        /// </summary>
        public override void Run()
        {
            Trace.TraceInformation("Checking for appropriate scale: '{0}'.", this.ServiceName);

            var timeToScale = this.units.Count == 0 ? 1 : 0;
            foreach (var unit in this.units)
            {
                foreach (IScalable t in unit.Tasks)
                {
                    timeToScale += t.Scale ? 1 : -1;
                }
            }

            if (timeToScale > 0 && this.units.Count < this.maximum) //Scale Up
            {
                Trace.TraceInformation("Scaling Up: '{0}'.", this.ServiceName);

                var unit = new RoleTaskManager<T>(this);
                this.units.Push(unit);

                unit.OnStart();
                unit.Run();

                Trace.TraceInformation("Scaled Up: '{0}'.", this.ServiceName);
            }
            else if (timeToScale < 0 && this.units.Count > this.minimum) //Scale Down
            {
                Trace.TraceInformation("Scaling Down: '{0}'.", this.ServiceName);

                IRoleTaskManager<T> unit;
                if (this.units.TryPop(out unit))
                {
                    unit.OnStop();
                    unit.Dispose();
                }

                Trace.TraceInformation("Scaled Down: '{0}'.", this.ServiceName);
            }
            else
            {
                Trace.TraceInformation("Currently running at optimal scale: '{0}'.", this.ServiceName);
            }
        }
        #endregion
    }
}
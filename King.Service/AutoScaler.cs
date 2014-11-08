namespace King.Service
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
        public AutoScaler(T configuration = default(T), byte minimum = 0, byte maximum = 1, byte scaleCheckInMinutes = 20)
            : base(BaseTimes.InitializationTiming, (int)TimeSpan.FromMinutes(scaleCheckInMinutes).TotalSeconds)
        {
            if (1 > minimum)
            {
                throw new ArgumentException("Minimum must be 1 or greater");
            }
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

            if (this.units.Count < this.minimum)
            {
                while (this.units.Count < this.minimum)
                {
                    this.ScaleUp();
                }
            }
            else
            {
                var timeToScale = 0;
                foreach (var unit in this.units)
                {
                    foreach (IScalable t in unit.Tasks)
                    {
                        timeToScale += t.Scale ? 1 : -1;
                    }
                }

                if (timeToScale > 0 && this.units.Count < this.maximum) //Scale Up
                {
                    this.ScaleUp();
                }
                else if (timeToScale < 0 && this.units.Count > this.minimum) //Scale Down
                {
                    this.ScaleDown();
                }
                else
                {
                    Trace.TraceInformation("'{0}' is currently running at optimal scale.", this.ServiceName);
                }
            }
        }

        /// <summary>
        /// Scale Up by one unit
        /// </summary>
        public virtual void ScaleUp()
        {
            Trace.TraceInformation("Scaling Up: '{0}'.", this.ServiceName);

            var unit = new RoleTaskManager<T>(this);

            var success = unit.OnStart();
            if (success)
            {
                unit.Run();

                this.units.Push(unit);

                Trace.TraceInformation("Scaled Up: '{0}'.", this.ServiceName);
            }
            else
            {
                unit.Dispose();

                Trace.TraceWarning("Failed to start Scale Unit: '{0}'.", this.ServiceName);
            }
        }

        /// <summary>
        /// Scale down by one unit
        /// </summary>
        public virtual void ScaleDown()
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
        #endregion
    }
}
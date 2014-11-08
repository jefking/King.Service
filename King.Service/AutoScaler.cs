namespace King.Service
{
    using King.Service.Scalability;
    using King.Service.Timing;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Task AutoScaler
    /// </summary>
    /// <typeparam name="T">Configuration</typeparam>
    public abstract class AutoScaler<T> : RecurringTask, ITaskFactory<T>
    {
        #region Members
        /// <summary>
        /// Minimum Instance Count
        /// </summary>
        protected readonly byte minimum = 1;

        /// <summary>
        /// Maximum Instance Count
        /// </summary>
        protected readonly byte maximum = 2;

        /// <summary>
        /// Configuration
        /// </summary>
        protected readonly T configuration = default(T);

        /// <summary>
        /// Running Tasks
        /// </summary>
        protected readonly ConcurrentStack<IRoleTaskManager<T>> units = new ConcurrentStack<IRoleTaskManager<T>>();

        /// <summary>
        /// Scaler
        /// </summary>
        protected readonly IScaler<T> scaler = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AutoScaler(T configuration = default(T), byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = 20)
            : this(new Scaler<T>(), configuration, minimum, maximum, checkScaleInMinutes)
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public AutoScaler(IScaler<T> scaler, T configuration = default(T), byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = 20)
            : base(BaseTimes.InitializationTiming, (int)TimeSpan.FromMinutes(checkScaleInMinutes).TotalSeconds)
        {
            if (1 > minimum)
            {
                throw new ArgumentException("Minimum must be 1 or greater");
            }
            if (minimum > maximum)
            {
                throw new ArgumentException("Minimum should be less than Maximum");
            }
            if (null == scaler)
            {
                throw new ArgumentNullException("Scaler");
            }

            this.configuration = configuration;
            this.minimum = minimum;
            this.maximum = maximum;
            this.scaler = scaler;
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
                    this.scaler.ScaleUp(this, this.units, this.ServiceName);
                }
            }
            else
            {
                var timeToScale = 0;
                foreach (IScalable t in this.units.SelectMany(u => u.Tasks))
                {
                    timeToScale += t.Scale ? 1 : -1;
                }

                if (timeToScale > 0 && this.units.Count < this.maximum)
                {
                    this.scaler.ScaleUp(this, this.units, this.ServiceName);
                }
                else if (timeToScale < 0 && this.units.Count > this.minimum)
                {
                    this.scaler.ScaleDown(this.units, this.ServiceName);
                }
                else
                {
                    Trace.TraceInformation("'{0}' is currently running at optimal scale with {1} units.", this.ServiceName, this.units.Count);
                }
            }
        }
        #endregion
    }
}
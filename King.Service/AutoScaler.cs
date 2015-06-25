namespace King.Service
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using King.Service.Scalability;
    using King.Service.Timing;

    /// <summary>
    /// Task Auto Scaler
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
        /// Scaler
        /// </summary>
        protected readonly IScaler<T> scaler = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="configuration">Configuration</param>
        /// <param name="minimum">Minimum Scale</param>
        /// <param name="maximum">Maximmum Scale</param>
        /// <param name="checkScaleInMinutes">Check Scale Every</param>
        public AutoScaler(T configuration = default(T), byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = BaseTimes.ScaleCheck)
            : this(new Scaler<T>(), configuration, minimum, maximum, checkScaleInMinutes)
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="scaler">Scaler</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="minimum">Minimum Scale</param>
        /// <param name="maximum">Maximmum Scale</param>
        /// <param name="checkScaleInMinutes">Check Scale Every</param>
        public AutoScaler(IScaler<T> scaler, T configuration = default(T), byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = BaseTimes.ScaleCheck)
            : base((int)TimeSpan.FromMinutes(checkScaleInMinutes).TotalSeconds)
        {
            if (minimum > maximum)
            {
                throw new ArgumentException("Minimum should be less than Maximum");
            }
            if (null == scaler)
            {
                throw new ArgumentNullException("Scaler");
            }

            this.configuration = configuration;
            this.minimum = 1 > minimum ? (byte)1 : minimum;
            this.maximum = 2 > maximum ? (byte)2 : maximum;
            this.scaler = scaler;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Minimum Instance Count
        /// </summary>
        public virtual byte Minimum
        {
            get
            {
                return this.minimum;
            }
        }

        /// <summary>
        /// Maximum Instance Count
        /// </summary>
        public virtual byte Maximum
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
        public virtual IEnumerable<IRunnable> Tasks(T data = default(T))
        {
            return this.ScaleUnit(this.configuration);
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
            Trace.TraceInformation("Checking for appropriate scale: '{0}'.", this.Name);

            if (this.scaler.IsFirstRun(this.minimum))
            {
                this.scaler.Initialize(this.minimum, this, this.configuration, this.Name);
            }
            else
            {
                var scale = this.scaler.ShouldScale();
                if (scale.HasValue)
                {
                    if (scale.Value && this.scaler.CurrentUnits < this.maximum)
                    {
                        this.scaler.ScaleUp(this, this.configuration, this.Name);
                    }
                    else if (!scale.Value && this.scaler.CurrentUnits > this.minimum)
                    {
                        this.scaler.ScaleDown(this.Name);
                    }
                }

                Trace.TraceInformation("'{0}' is at optimal scale ({1} units).", this.Name, this.scaler.CurrentUnits);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">Disposing</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != this.scaler)
                {
                    this.scaler.Dispose();
                }
            }

            base.Dispose(disposing);
        }
        #endregion
    }
}
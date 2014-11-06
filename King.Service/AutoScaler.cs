namespace King.Service
{
    using System;
    using System.Linq;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

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
        /// <param name="minimum">Minimum</param>
        /// <param name="maximum">Maximum</param>
        public AutoScaler(byte minimum = 0, byte maximum = 1, T configuration = default(T))
        {
            if (minimum > maximum)
            {
                throw new ArgumentException("Minimum should be less than Maximum");
            }

            this.minimum = minimum;
            this.maximum = maximum;
            this.configuration = configuration;
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
        public abstract IEnumerable<IRunnable> Tasks(T data);

        /// <summary>
        /// Task Run
        /// </summary>
        public override void Run()
        {
            var timeToScale = 0;
            foreach (var unit in this.units)
            {
                //Determine a way to track if processes are doing work.
                //timeToScale += task.Busy ? 1 : -1;
            }

            if (timeToScale > 0 && this.units.Count > this.minimum) //Scale Up
            {
                var unit = new RoleTaskManager<T>(this);
                unit.OnStart();
                unit.Run();
                this.units.Push(unit);
            }
            else if (timeToScale < 0 && this.units.Count < this.maximum) //Scale Down
            {
                IRoleTaskManager<T> unit;
                if (this.units.TryPop(out unit))
                {
                    unit.OnStop();
                }
            }
        }
        #endregion
    }
}
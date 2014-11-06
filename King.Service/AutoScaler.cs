namespace King.Service
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Task AutoScaler
    /// </summary>
    public abstract class AutoScaler : TaskManager, IAutoScaler
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

        protected readonly Stack<IEnumerable<IRunnable>> units = new Stack<IEnumerable<IRunnable>>();
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="minimum">Minimum</param>
        /// <param name="maximum">Maximum</param>
        public AutoScaler(byte minimum = 0, byte maximum = 1)
        {
            if (minimum > maximum)
            {
                throw new ArgumentException("Minimum should be less than Maximum");
            }

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
        /// Scale Unit
        /// </summary>
        /// <returns>Tasks to run</returns>
        public abstract IEnumerable<IRunnable> ScaleUnit<T>(T data);

        /// <summary>
        /// Task Run
        /// </summary>
        public override void Run()
        {
            var timeToScale = 0;
            foreach (var unit in this.units)
            {
                foreach (var task in unit)
                {
                    //timeToScale += task.Busy ? 1 : -1;
                }
            }
            if (timeToScale > 0)
            {
                var unit = this.ScaleUnit<object>(null);
                foreach (var u in unit)
                {
                    u.Start();
                }
                this.units.Push(unit);
            }
            else if (timeToScale < 0)
            {
                var unit = this.units.Pop();
                foreach(var u in unit)
                {
                    u.Stop();
                }
            }
        }
        #endregion
    }
}
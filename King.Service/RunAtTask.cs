namespace King.Service
{
    using System;

    /// <summary>
    /// Run at specified time daily (once)
    /// </summary>
    public abstract class RunAtTask : CoordinatedTask
    {
        #region Members
        /// <summary>
        /// Hour to execute on
        /// </summary>
        protected readonly byte hour = 0;

        /// <summary>
        /// Minute to execute on
        /// </summary>
        protected readonly sbyte minute = -1;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connectionString">Coordinated Task Connection String</param>
        /// <param name="hour">Hour to run</param>
        /// <param name="minute">Minute to run (-1 for on the hour)</param>
        public RunAtTask(string connectionString, byte hour = 0, sbyte minute = -1)
            : base(connectionString, minute <= 0 ? 3599 : 59)
        {
            this.hour = hour > 23 ? (byte)23 : hour;
            this.minute = minute < 0 ? (sbyte)-1 : (minute > 59 ? (sbyte)59 : (sbyte)minute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Hour to execute on
        /// </summary>
        public virtual byte Hour
        {
            get
            {
                return this.hour;
            }
        }

        /// <summary>
        /// Minute to execute on
        /// </summary>
        public virtual sbyte Minute
        {
            get
            {
                return this.minute;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run at correct time
        /// </summary>
        public override void Run()
        {
            var now = DateTime.UtcNow;
            if (now.Hour == this.Hour && (-1 == this.Minute || now.Minute == this.Minute))
            {
                this.Run(now);
            }
        }

        /// <summary>
        /// Run
        /// </summary>
        /// <param name="currentTime">Primarily for testing</param>
        /// <returns>Task</returns>
        public abstract void Run(DateTime currentTime);
        #endregion
    }
}
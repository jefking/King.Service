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
        protected readonly byte minute = 0;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connectionString">Coordinated Task Connection String</param>
        /// <param name="hour">Hour to run</param>
        /// <param name="minute">Minute to run (-1 for on the hour)</param>
        public RunAtTask(string connectionString, byte hour = 0, sbyte minute = -1)
            : base(connectionString, minute <= 0 ? 3600 : 60)
        {
            this.hour = hour < 0 ? (byte)0 : (hour > 23 ? (byte)23 : hour);
            this.minute = minute < 0 ? (byte)0 : (minute > 59 ? (byte)59 : (byte)minute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Hour to execute on
        /// </summary>
        public byte Hour
        {
            get
            {
                return this.hour;
            }
        }

        /// <summary>
        /// Minute to execute on
        /// </summary>
        public byte Minute
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
            if (now.Hour == this.hour)
            {
                if (0 == this.minute || now.Minute == this.minute)
                {
                    this.Run(now);
                }
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
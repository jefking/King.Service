namespace King.Service
{
    using King.Service.Timing;
    using System;

    /// <summary>
    /// Initialize Task
    /// </summary>
    public abstract class InitializeTask : TaskManager
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public InitializeTask()
            : base(BaseTimes.InitializationTiming, BaseTimes.NoRepeat)
        {
        }
        #endregion
    }
}
namespace King.Service
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Timing;

    /// <summary>
    /// Between Runs
    /// </summary>
    public class BetweenRuns : IDynamicRuns
    {
        #region Members
        /// <summary>
        /// Minimum Period in Seconds
        /// </summary>
        public virtual int MinimumPeriodInSeconds
        {
            get;
            protected set;
        }

        /// <summary>
        /// Maximum Period in Seconds
        /// </summary>
        public virtual int MaximumPeriodInSeconds
        {
            get;
            protected set;
        }

        /// <summary>
        /// Instance
        /// </summary>
        protected readonly object instance;

        /// <summary>
        /// Method
        /// </summary>
        protected readonly MethodInfo method;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="instance">Instance</param>
        /// <param name="method">Method</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period in Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period in Seconds</param>
        public BetweenRuns(object instance, MethodInfo method, int minimumPeriodInSeconds = BaseTimes.DefaultMinimumTiming, int maximumPeriodInSeconds = BaseTimes.DefaultMaximumTiming)
        {
            if (null == instance)
            {
                throw new ArgumentNullException("Instance can not be null.");
            }
            if (null == method)
            {
                throw new ArgumentNullException("Method for invocation can not be null.");
            }
            if (0 >= minimumPeriodInSeconds)
            {
                throw new ArgumentException("Frequency must be greater than 0.");
            }
            if (minimumPeriodInSeconds > maximumPeriodInSeconds)
            {
                throw new ArgumentException("Frequency must be greater than 0.");
            }

            this.instance = instance;
            this.method = method;
            this.MinimumPeriodInSeconds = minimumPeriodInSeconds;
            this.MaximumPeriodInSeconds = maximumPeriodInSeconds;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run
        /// </summary>
        /// <returns>Work was done</returns>
        public virtual Task<bool> Run()
        {
            var result = this.method.Invoke(instance, null);
            return Task.FromResult<bool>((bool)result);
        }
        #endregion
    }
}
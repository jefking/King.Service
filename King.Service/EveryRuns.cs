namespace King.Service
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Timing;

    /// <summary>
    /// Every Runs
    /// </summary>
    public class EveryRuns : IRuns
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
        public EveryRuns(object instance, MethodInfo method, int minimumPeriodInSeconds = BaseTimes.DefaultMinimumTiming)
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

            this.instance = instance;
            this.method = method;
            this.MinimumPeriodInSeconds = minimumPeriodInSeconds;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run
        /// </summary>
        /// <returns>Success</returns>
        public virtual Task<bool> Run()
        {
            this.method.Invoke(instance, null);

            return Task.FromResult<bool>(true);
        }
        #endregion
    }
}
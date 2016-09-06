namespace King.Service
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Initialize Runner
    /// </summary>
    public class InitializeRunner : InitializeTask
    {
        #region Members
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
        public InitializeRunner(object instance, MethodInfo method)
        {
            if (null == instance)
            {
                throw new ArgumentNullException("Instance can not be null.");
            }
            if (null == method)
            {
                throw new ArgumentNullException("Method for invocation can not be null.");
            }

            this.instance = instance;
            this.method = method;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run
        /// </summary>
        public override void Run()
        {
            var result = this.method.Invoke(instance, null);
        }
        #endregion
    }
}
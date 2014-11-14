namespace King.Service
{
    using System.Threading.Tasks;

    /// <summary>
    /// Initialize Task
    /// </summary>
    public class InitializeTask : IRunnable
    {
        #region Methods
        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="state">State of Timer</param>
        public void Run(object state)
        {
            this.Run();
        }

        /// <summary>
        /// Run Method
        /// </summary>
        public virtual void Run()
        {
            this.RunAsync().Wait();
        }

        /// <summary>
        /// Run Async
        /// </summary>
        /// <returns></returns>
        public virtual async Task RunAsync()
        {
            await new TaskFactory().StartNew(() => { });
        }

        /// <summary>
        /// Stops Service
        /// </summary>
        /// <returns>Stopped</returns>
        public virtual bool Stop()
        {
            return true;
        }

        /// <summary>
        /// Runs Service
        /// </summary>
        /// <returns>Running</returns>
        public virtual bool Start()
        {
            return true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
        }
        #endregion
    }
}
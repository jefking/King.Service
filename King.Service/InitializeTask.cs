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
            this.Run();

            return true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
        }
        #endregion
    }
}
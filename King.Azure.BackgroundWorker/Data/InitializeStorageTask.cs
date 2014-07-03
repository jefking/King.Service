namespace King.Azure.BackgroundWorker.Data
{
    using King.Azure.BackgroundWorker;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Initialize Storage
    /// </summary>
    public class InitializeStorageTask : InitializeTask
    {
        #region Members
        /// <summary>
        /// Azure Storage
        /// </summary>
        private readonly IAzureStorage storage = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Intialize class with Storage to create
        /// </summary>
        /// <param name="storage">Storage</param>
        public InitializeStorageTask(IAzureStorage storage)
        {
            if (null == storage)
            {
                throw new ArgumentNullException("storage");
            }

            this.storage = storage;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run, Creates Table
        /// </summary>
        public override void Run()
        {
            Trace.TraceInformation("Creating: '{0}'.", this.storage.Name);

            var task = this.storage.CreateIfNotExists();
            task.Wait();

            Trace.TraceInformation("Created: '{0}'; was created {1}.", this.storage.Name, task.Result);
        }
        #endregion
    }
}
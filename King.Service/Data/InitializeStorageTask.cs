namespace King.Service.Data
{
    using King.Service;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Initialize Storage Task
    /// </summary>
    public class InitializeStorageTask : InitializeTask
    {
        #region Members
        /// <summary>
        /// Azure Storage
        /// </summary>
        protected readonly IAzureStorage storage = null;
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
        public override async Task RunAsync()
        {
            Trace.TraceInformation("Creating: '{0}'.", this.storage.Name);

            var result = await this.storage.CreateIfNotExists();

            Trace.TraceInformation("Created: '{0}'; was created {1}.", this.storage.Name, result);
        }
        #endregion
    }
}
namespace King.Azure.BackgroundWorker.Data
{
    using King.Azure.BackgroundWorker;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Initialize table for Coordination
    /// </summary>
    public class InitializeTableTask : InitializeTask
    {
        #region Members
        /// <summary>
        /// Table Storage
        /// </summary>
        private readonly ITableStorage table = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Intialize class with Table to create
        /// </summary>
        /// <param name="table"></param>
        public InitializeTableTask(ITableStorage table)
        {
            if (null == table)
            {
                throw new ArgumentNullException("table");
            }

            this.table = table;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run, Creates Table
        /// </summary>
        public override void Run()
        {
            Trace.TraceInformation("Creating Table: '{0}'.", this.table.Name);

            var task = this.table.CreateIfNotExists();
            task.Wait();

            Trace.TraceInformation("Created Table: '{0}'; was created {1}.", this.table.Name, task.Result);
        }
        #endregion
    }
}
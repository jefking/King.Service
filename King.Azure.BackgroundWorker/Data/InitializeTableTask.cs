namespace King.Azure.BackgroundWorker.Data
{
    using King.Azure.BackgroundWorker;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Initialize table for Coordination
    /// </summary>
    public class InitializeTableTask : Manager
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
            : base(10, TimeSpan.FromDays(7).Seconds)
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
            var task = this.table.CreateIfNotExists();
            task.Wait();
        }
        #endregion
    }
}
namespace King.Azure.BackgroundWorker.Data
{
    using King.Azure.BackgroundWorker;
    using King.Azure.BackgroundWorker.Data.Azure;
    using System;

    public class InitializeTableTask : Manager
    {
        #region Members
        private readonly ITableStorage table = null;
        #endregion

        #region Constructors
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
        public override void Run()
        {
            var task = this.table.CreateIfNotExists();
            task.Wait();
        }
        #endregion
    }
}
namespace King.Azure.BackgroundWorker.Data
{
    using King.Azure.BackgroundWorker.Data.Model;
    using System.Threading.Tasks;

    /// <summary>
    /// Scheduled Task Core for Mocking
    /// </summary>
    public interface ICoordinator
    {
        #region Methods
        /// <summary>
        /// Initialize Table
        /// </summary>
        Manager InitializeTask();

        /// <summary>
        /// Determine whether a new task needs to be executed
        /// </summary>
        /// <param name="entry">Scheduled Task Entry</param>
        /// <returns>True if need to execute, false if not</returns>
        bool CheckForTask(ScheduledTaskEntry entry);
        #endregion
    }
}
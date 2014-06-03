namespace King.Service.Data
{
    using System.Threading.Tasks;
    using King.Service.Data.Model;

    /// <summary>
    /// Scheduled Task Core for Mocking
    /// </summary>
    public interface IScheduledTaskCore
    {
        #region Methods
        /// <summary>
        /// Initialize Table
        /// </summary>
        Task<bool> InitializeTable();

        /// <summary>
        /// Determine whether a new task needs to be executed
        /// </summary>
        /// <param name="entry">Scheduled Task Entry</param>
        /// <returns>True if need to execute, false if not</returns>
        bool CheckForTask(ScheduledTaskEntry entry);
        #endregion
    }
}
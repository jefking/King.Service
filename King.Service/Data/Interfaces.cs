namespace King.Service.Data
{
    using Microsoft.WindowsAzure.Storage.Queue;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
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
        IRunnable InitializeTask();

        /// <summary>
        /// Determine whether a new task needs to be executed
        /// </summary>
        /// <param name="entry">Scheduled Task Entry</param>
        /// <returns>True if need to execute, false if not</returns>
        Task<bool> Check(Type type);

        /// <summary>
        /// Start Task
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        Task Start(Type type, Guid identifier, DateTime start);

        /// <summary>
        /// Complete Task
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        Task Complete(Type type, Guid identifier, DateTime start, DateTime end, bool success);
        #endregion

        #region Properties
        int PeriodInSeconds
        {
            get;
        }
        #endregion
    }
}
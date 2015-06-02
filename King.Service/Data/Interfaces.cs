namespace King.Service.Data
{
    using System;
    using System.Threading.Tasks;
    using King.Azure.Data;

    #region ICoordinator
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
        /// <param name="type">Task Type</param>
        /// <param name="identifier">Identifier</param>
        /// <param name="start">Start</param>
        /// <returns>Task</returns>
        Task Start(Type type, Guid identifier, DateTime start);

        /// <summary>
        /// Complete Task
        /// </summary>
        /// <param name="type">Task Type</param>
        /// <param name="identifier">Identifier</param>
        /// <param name="start">Start</param>
        /// <param name="end">End</param>
        /// <param name="success">Success</param>
        /// <returns>Task</returns>
        Task Complete(Type type, Guid identifier, DateTime start, DateTime end, bool success);
        #endregion

        #region Properties
        /// <summary>
        /// Period in seconds
        /// </summary>
        int PeriodInSeconds
        {
            get;
        }
        #endregion
    }
    #endregion

    #region IQueueSetup
    /// <summary>
    /// Queue Setup Interface
    /// </summary>
    /// <typeparam name="T">Dequeue Type</typeparam>
    public interface IQueueSetup<T>
    {
        #region Properties
        /// <summary>
        /// Name
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Connection String
        /// </summary>
        string ConnectionString
        {
            get;
        }

        /// <summary>
        /// Priority
        /// </summary>
        QueuePriority Priority
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get Processor
        /// </summary>
        /// <returns>Processor</returns>
        IProcessor<T> Get();
        #endregion
    }
    #endregion
}
namespace King.Service.Data
{
    using System;
    using System.Collections.Generic;
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
        /// Priority
        /// </summary>
        QueuePriority Priority
        {
            get;
        }

        /// <summary>
        /// Get Processor
        /// </summary>
        Func<IProcessor<T>> Processor
        {
            get;
        }
        #endregion
    }
    #endregion

    #region IQueueConnection
    /// <summary>
    /// Queue Connection Interface
    /// </summary>
    /// <typeparam name="T">Dequeue Model</typeparam>
    public interface IQueueConnection<T>
    {
        #region Properties
        /// <summary>
        /// Connection String
        /// </summary>
        string ConnectionString
        {
            get;
        }

        /// <summary>
        /// Queue Setup
        /// </summary>
        IQueueSetup<T> Setup
        {
            get;
        }
        #endregion
    }
    #endregion

    #region IDequeueFactory
    /// <summary>
    /// Dequeue Factory
    /// </summary>
    public interface IDequeueFactory
    {
        #region Methods
        /// <summary>
        /// Initializes the Queues, and Dequeuers
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="setups">Setups</param>
        /// <returns>Tasks</returns>
        IEnumerable<IRunnable> Tasks<T>(IEnumerable<IQueueSetup<T>> setups);

        /// <summary>
        /// Initializes the Queue, and Dequeuer
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="setup">Setup</param>
        /// <returns>Tasks</returns>
        IEnumerable<IRunnable> Tasks<T>(IQueueSetup<T> setup);

        /// <summary>
        /// Dequeue Task (Storage Queue Auto Scaler)
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="setup">Setup</param>
        /// <returns>Storage Queue Auto Scaler</returns>
        IRunnable Dequeue<T>(IQueueSetup<T> setup);
        #endregion
    }
    #endregion
}
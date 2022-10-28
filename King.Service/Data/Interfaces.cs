namespace King.Service.Data
{
    using global::Azure.Data.Wrappers;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    #region ICoordinator
    /// <summary>
    /// Scheduled Task Core for Mocking
    /// </summary>
    public interface ICoordinator : ITaskFactory<object>
    {
        #region Methods
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
        /// Create Multiple Queues
        /// </summary>
        /// <param name="names">Names</param>
        /// <returns>Initialization Tasks</returns>
        IEnumerable<IRunnable> Initialize(params string[] names);

        /// <summary>
        /// Create Queue
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Initialization Tasks</returns>
        IRunnable Initialize(string name);

        /// <summary>
        /// Initializes the Queues, and Dequeuers
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="queueName">Queue Name</param>
        /// <param name="processor">Processor</param>
        /// <param name="priority">Priority</param>
        /// <returns>Tasks</returns>
        IEnumerable<IRunnable> Tasks<T>(string queueName, Func<IProcessor<T>> processor, QueuePriority priority = QueuePriority.Low);

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

        /// <summary>
        /// Dequeue Task Set
        /// </summary>
        /// <typeparam name="Y">Processor</typeparam>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="name">Name</param>
        /// <param name="priority">Priority</param>
        /// <returns></returns>
        IEnumerable<IRunnable> Dequeue<T, Y>(string name, QueuePriority priority = QueuePriority.Low)
            where T : IProcessor<Y>, new();
        #endregion
    }
    #endregion
}
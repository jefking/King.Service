namespace King.Service.Data
{
    using Azure;
    using King.Service.Data.Model;
    using King.Service.Scalability;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Dequeue Factory
    /// </summary>
    public class DequeueFactory : IDequeueFactory
    {
        #region Members
        /// <summary>
        /// Connection String
        /// </summary>
        protected readonly string connectionString = null;

        /// <summary>
        /// Throughput
        /// </summary>
        protected readonly IQueueThroughput throughput = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        public DequeueFactory(string connectionString)
            : this(connectionString, new QueueThroughput())
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="throughput">Queue Throughput</param>
        public DequeueFactory(string connectionString, IQueueThroughput throughput = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("connectionString");
            }

            this.connectionString = connectionString;
            this.throughput = throughput ?? new QueueThroughput();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create Multiple Queues
        /// </summary>
        /// <param name="names">Names</param>
        /// <returns>Initialization Tasks</returns>
        public virtual IEnumerable<IRunnable> Initialize(params string[] names)
        {
            if (null == names)
            {
                throw new ArgumentNullException("names");
            }

            foreach (var name in names.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                yield return this.Initialize(name);
            }
        }

        /// <summary>
        /// Create Queue
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Initialization Tasks</returns>
        public virtual IRunnable Initialize(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }

            return new InitializeStorageTask(new StorageQueue(name, this.connectionString));
        }

        /// <summary>
        /// Initializes the Queues, and Dequeuers
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="queueName">Queue Name</param>
        /// <param name="processor">Processor</param>
        /// <param name="priority">Priority</param>
        /// <returns>Tasks</returns>
        public virtual IEnumerable<IRunnable> Tasks<T>(string queueName, Func<IProcessor<T>> processor, QueuePriority priority = QueuePriority.Low)
        {
            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentException("queueName");
            }
            if (null == processor)
            {
                throw new ArgumentNullException("processor");
            }

            return this.Tasks<T>(new QueueSetup<T>
            {
                Processor = processor,
                Priority = priority,
                Name = queueName,
            });
        }

        /// <summary>
        /// Initializes the Queues, and Dequeuers
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="setups">Setups</param>
        /// <returns>Tasks</returns>
        public virtual IEnumerable<IRunnable> Tasks<T>(IEnumerable<IQueueSetup<T>> setups)
        {
            if (null == setups)
            {
                throw new ArgumentNullException("setups");
            }

            foreach (var setup in setups)
            {
                foreach (var t in this.Tasks(setup))
                {
                    yield return t;
                }
            }
        }

        /// <summary>
        /// Initializes the Queue, and Dequeuer
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="setup">Setup</param>
        /// <returns>Tasks</returns>
        public virtual IEnumerable<IRunnable> Tasks<T>(IQueueSetup<T> setup)
        {
            if (null == setup)
            {
                throw new ArgumentNullException("setup");
            }

            yield return this.Initialize(setup.Name);
            yield return this.Dequeue<T>(setup);
        }

        /// <summary>
        /// Dequeue Task (Storage Queue Auto Scaler)
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="setup">Setup</param>
        /// <returns>Storage Queue Auto Scaler</returns>
        public virtual IRunnable Dequeue<T>(IQueueSetup<T> setup)
        {
            if (null == setup)
            {
                throw new ArgumentNullException("setup");
            }

            var messagesPerScaleUnit = this.throughput.MessagesPerScaleUnit(setup.Priority);
            var scale = this.throughput.Scale(setup.Priority);
            var checkScaleInMinutes = this.throughput.CheckScaleEvery(setup.Priority);
            var connection = new QueueConnection<T>()
            {
                ConnectionString = this.connectionString,
                Setup = setup,
            };

            return new StorageQueueAutoScaler<T>(new StorageQueue(connection.Setup.Name, connection.ConnectionString)
                , connection
                , this.throughput
                , messagesPerScaleUnit
                , scale.Minimum
                , scale.Maximum
                , checkScaleInMinutes);
        }

        /// <summary>
        /// Dequeue Task Set
        /// </summary>
        /// <typeparam name="Y">Processor</typeparam>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="name">Queue Name</param>
        /// <param name="priority">Priority</param>
        /// <returns>Dequeue Tasks</returns>
        public virtual IEnumerable<IRunnable> Dequeue<T, Y>(string name, QueuePriority priority = QueuePriority.Low)
            where T : IProcessor<Y>, new()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }

            return this.Tasks<Y>(new QueueSetupProcessor<T, Y>
            {
                Priority = priority,
                Name = name,
            });
        }

        /// <summary>
        /// Dequeue, from shards
        /// </summary>
        /// <remarks>
        /// This should only be used when you need to pass high volumes of data. Less than 2,000 per shard per second
        /// </remarks>
        /// <typeparam name="Y">Processor</typeparam>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="name">Queue Name</param>
        /// <param name="shardCount">Shard Count</param>
        /// <param name="priority">Priority</param>
        /// <returns>Dequeue Tasks</returns>
        public virtual IEnumerable<IRunnable> Shards<T, Y>(string name, byte shardCount = 2, QueuePriority priority = QueuePriority.High)
            where T : IProcessor<Y>, new()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }

            shardCount = shardCount > 0 ? shardCount : (byte)2;

            var qs = new List<IRunnable>();
            for (var i = 0; i < shardCount; i++)
            {
                var n = string.Format("{0}{1}", name, i);
                qs.AddRange(this.Dequeue<T, Y>(n, priority));
            }

            return qs;
        }
        #endregion
    }
}
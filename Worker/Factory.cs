namespace Worker
{
    using King.Azure.Data;
    using King.Service;
    using King.Service.Data;
    using King.Service.WorkerRole;
    using System.Collections.Generic;
    using Worker.Queue;
    using Worker.Scalable;

    /// <summary>
    /// Task Factory
    /// </summary>
    public class Factory : ITaskFactory<Configuration>
    {
        public IEnumerable<IRunnable> Tasks(Configuration config)
        {
            // Initialization task
            yield return new InitTask();

            // Initialize Table; creates table if it doesn't already exist
            var table = new TableStorage(config.TableName, config.ConnectionString);
            yield return new InitializeStorageTask(table);

            // Initialize Queue; creates queue if it doesn't already exist
            var queue = new StorageQueue(config.QueueName, config.ConnectionString);
            yield return new InitializeStorageTask(queue);

            // Initialize Container; creates container if it doesn't already exist
            var container = new Container(config.ContainerName, config.ConnectionString);
            yield return new InitializeStorageTask(container);

            //basic task
            yield return new Recurring();

            //Cordinated Tasks between Instances
            var task = new Coordinated(config.ConnectionString);

            // Add once to ensure that Table is created for Instances to communicate with
            yield return task.InitializeTask();

            // Add your coordinated task
            yield return task;

            //Task once daily on the (current) hour
            yield return new OnceDaily(config.ConnectionString);

            //Backoff task
            yield return new Backoff();

            //Self Governing task
            yield return new Adaptive();

            //Dequeue task, Backoff behavior
            yield return new BackoffRunner(new CompanyDequeuer(queue));

            //Dequeue task, Adaptive behavior
            yield return new AdaptiveRunner(new CompanyDequeuer(queue));

            //Task for Queuing
            yield return new CompanyQueuer();

            //Auto Scaling Task
            yield return new DynamicScaler(config);
        }
    }
}
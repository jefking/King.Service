namespace Worker
{
    using King.Azure.Data;
    using King.Service;
    using King.Service.Data;
    using System.Collections.Generic;
    using Worker.Queue;

    public class Factory : ITaskFactory<Configuration>
    {
        public IEnumerable<IRunnable> Tasks(Configuration config)
        {
            // Initialization Task(s)
            yield return new InitTask();

            // Initialize Table; creates table if it doesn't already exist
            var table = new TableStorage(config.TableName, config.ConnectionString);
            yield return new InitializeStorageTask(table);

            // Initialize Queue; creates queue if it doesn't already exist
            var queue = new StorageQueue(config.QueueName, config.ConnectionString);
            yield return new InitializeStorageTask(queue);

            // Initialize Queue; creates queue if it doesn't already exist
            var container = new Container(config.ContainerName, config.ConnectionString);
            yield return new InitializeStorageTask(container);

            //Task(s)
            yield return new Task();

            //Cordinated Tasks between Instances
            var task = new Coordinated(config.ConnectionString);

            // Add once to ensure that Table is created for Instances to communicate with
            yield return task.InitializeTask();

            // Add your coordinated task(s)
            yield return task;

            //Backoff task(s)
            yield return new Backoff();

            //Self Governing task(s)
            yield return new Adaptive();

            //Dequeue task(s), Backoff
            yield return new BackoffRunner(new CompanyDequeuer(queue));

            //Dequeue task(s), Adaptive
            yield return new AdaptiveRunner(new CompanyDequeuer(queue));

            //Task for Queuing
            yield return new CompanyQueuer();

            yield return new OnceDaily(config.ConnectionString);
        }
    }
}
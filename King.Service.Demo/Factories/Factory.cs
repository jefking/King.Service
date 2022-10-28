namespace King.Service.Demo.Factories
{
    using King.Service;
    using King.Service.Data;
    using King.Service.Demo.Models;
    using King.Service.Demo.Processors;
    using King.Service.Demo.Tasks;
    using System.Collections.Generic;

    /// <summary>
    /// Builds manifest of tasks to run
    /// </summary>
    public class Factory : ITaskFactory<AppConfig>
    {
        public IEnumerable<IRunnable> Tasks(AppConfig config)
        {
            // List of tasks to run
            var tasks = new List<IRunnable>(new IRunnable[] {
                // Initialization task
                new InitTask(),

                // Initialize Table; creates table if it doesn't already exist
                new InitializeStorageTask(new TableStorage(config.TableName, config.ConnectionString)),

                // Initialize Container; creates container if it doesn't already exist
                new InitializeStorageTask(new Container(config.ContainerName, config.ConnectionString)),

                // Initialize Container; creates container if it doesn't already exist
                new InitializeStorageTask(new StorageQueue(config.GenericQueueName, config.ConnectionString)),

                //basic task
                new Recurring(),

                //Task once daily on the (specified/current) hour
                new OnceDaily(config.ConnectionString),

                //Backoff task
                new Backoff(),

                //Self governing task
                new Adaptive(),

                //Dequeue task, Backoff behavior
                new BackoffRunner(new CompanyDequeuer(config.GenericQueueName, config.ConnectionString)),

                //Dequeue task, Adaptive behavior
                new AdaptiveRunner(new CompanyDequeuer(config.GenericQueueName, config.ConnectionString)),

                //Dequeue task, Recurring behavior
                new RecurringRunner(new CompanyDequeuer(config.GenericQueueName, config.ConnectionString)),

                //Auto Scaling Task
                new DynamicScaler(config),
            });

            ///Dequeue examples
            var f = new DequeueFactory(config.ConnectionString);
            tasks.AddRange(f.Dequeue<CompanyProcessor, CompanyModel>(config.SlowQueueName));
            tasks.AddRange(f.Dequeue<CompanyProcessor, CompanyModel>(config.ModerateQueueName, QueuePriority.Medium));
            tasks.AddRange(f.Dequeue<CompanyProcessor, CompanyModel>(config.FastQueueName, QueuePriority.High));
            tasks.AddRange(f.Shards<CompanyProcessor, CompanyModel>(config.ShardQueueName, 10));

            //Cordinated Tasks between Instances
            var coordinated = new Coordinated(config.ConnectionString);
            // Add once to ensure that Table is created for Instances to communicate with
            tasks.AddRange(coordinated.Tasks());
            // Add your coordinated task
            tasks.Add(coordinated);

            return tasks;
        }
    }
}
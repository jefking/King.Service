namespace King.Service.CloudService.Role
{
    using King.Azure.Data;
    using King.Service;
    using King.Service.CloudService.Role.Queue;
    using King.Service.CloudService.Role.Scalable;
    using King.Service.Data;
    using System.Collections.Generic;

    public class Factory : ITaskFactory<Configuration>
    {
        public IEnumerable<IRunnable> Tasks(Configuration config)
        {
            var tasks = new List<IRunnable>();

            // Initialization task
            tasks.Add(new InitTask());

            // Initialize Table; creates table if it doesn't already exist
            tasks.Add(new InitializeStorageTask(new TableStorage(config.TableName, config.ConnectionString)));

            // Initialize Container; creates container if it doesn't already exist
            tasks.Add(new InitializeStorageTask(new Container(config.ContainerName, config.ConnectionString)));

            //basic task
            tasks.Add(new Recurring());

            //Cordinated Tasks between Instances
            var task = new Coordinated(config.ConnectionString);

            // Add once to ensure that Table is created for Instances to communicate with
            tasks.AddRange(task.Tasks());

            // Add your coordinated task
            tasks.Add(task);

            //Task once daily on the (specified/current) hour
            tasks.Add(new OnceDaily(config.ConnectionString));

            //Backoff task
            tasks.Add(new Backoff());

            //Self governing task
            tasks.Add(new Adaptive());

            //Dequeue task, Backoff behavior
            tasks.Add(new BackoffRunner(new CompanyDequeuer(new StorageQueue(config.GenericQueueName, config.ConnectionString))));

            //Dequeue task, Adaptive behavior
            tasks.Add(new AdaptiveRunner(new CompanyDequeuer(new StorageQueue(config.GenericQueueName, config.ConnectionString))));

            //Dequeue task, Recurring behavior
            tasks.Add(new RecurringRunner(new CompanyDequeuer(new StorageQueue(config.GenericQueueName, config.ConnectionString))));

            //Auto Scaling Task
            tasks.Add(new DynamicScaler(config));
            
            ///Dequeue Tasks Example
            var f = new DequeueFactory(config.ConnectionString);
            tasks.AddRange(f.Dequeue<CompanyProcessor, CompanyModel>(config.SlowQueueName));
            tasks.AddRange(f.Dequeue<CompanyProcessor, CompanyModel>(config.ModerateQueueName, QueuePriority.Medium));
            tasks.AddRange(f.Dequeue<CompanyProcessor, CompanyModel>(config.FastQueueName, QueuePriority.High));

            //Tasks for Queuing (Demo purposes)
            tasks.Add(new CompanyQueuer(new StorageQueue(config.GenericQueueName, config.ConnectionString)));
            tasks.Add(new CompanyQueuer(new StorageQueue(config.FastQueueName, config.ConnectionString)));
            tasks.Add(new CompanyQueuer(new StorageQueue(config.ModerateQueueName, config.ConnectionString)));
            tasks.Add(new CompanyQueuer(new StorageQueue(config.SlowQueueName, config.ConnectionString)));

            return tasks;
        }
    }
}
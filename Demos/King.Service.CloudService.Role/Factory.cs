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
            // Initialization task
            yield return new InitTask();

            // Initialize Table; creates table if it doesn't already exist
            var table = new TableStorage(config.TableName, config.ConnectionString);
            yield return new InitializeStorageTask(table);

            // Initialize Container; creates container if it doesn't already exist
            var container = new Container(config.ContainerName, config.ConnectionString);
            yield return new InitializeStorageTask(container);

            //basic task
            yield return new Recurring();

            //Cordinated Tasks between Instances
            var task = new Coordinated(config.ConnectionString);

            // Add once to ensure that Table is created for Instances to communicate with
            foreach (var t in task.Tasks())
            {
                yield return t;
            }

            // Add your coordinated task
            yield return task;

            //Task once daily on the (specified/current) hour
            yield return new OnceDaily(config.ConnectionString);

            //Backoff task
            yield return new Backoff();

            //Self governing task
            yield return new Adaptive();

            //Dequeue task, Backoff behavior
            yield return new BackoffRunner(new CompanyDequeuer(new StorageQueue(config.GenericQueueName, config.ConnectionString)));

            //Dequeue task, Adaptive behavior
            yield return new AdaptiveRunner(new CompanyDequeuer(new StorageQueue(config.GenericQueueName, config.ConnectionString)));

            //Dequeue task, Recurring behavior
            yield return new RecurringRunner(new CompanyDequeuer(new StorageQueue(config.GenericQueueName, config.ConnectionString)));

            //Auto Scaling Task
            yield return new DynamicScaler(config);
            
            var f = new DequeueFactory(config.ConnectionString);
            foreach (var t in f.Dequeue<CompanyProcessor, CompanyModel>(config.SlowQueueName))
            {
                yield return t;
            }
            foreach (var t in f.Dequeue<CompanyProcessor, CompanyModel>(config.ModerateQueueName, QueuePriority.Medium))
            {
                yield return t;
            }
            foreach (var t in f.Dequeue<CompanyProcessor, CompanyModel>(config.FastQueueName, QueuePriority.High))
            {
                yield return t;
            }

            //Tasks for Queuing (Demo purposes)
            yield return new CompanyQueuer(new StorageQueue(config.GenericQueueName, config.ConnectionString));
            yield return new CompanyQueuer(new StorageQueue(config.FastQueueName, config.ConnectionString));
            yield return new CompanyQueuer(new StorageQueue(config.ModerateQueueName, config.ConnectionString));
            yield return new CompanyQueuer(new StorageQueue(config.SlowQueueName, config.ConnectionString));
        }
    }
}
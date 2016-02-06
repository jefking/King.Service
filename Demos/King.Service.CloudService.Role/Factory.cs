namespace King.Service.CloudService.Role
{
    using System.Collections.Generic;
    using King.Azure.Data;
    using King.Service;
    using King.Service.CloudService.Role.Queue;
    using King.Service.CloudService.Role.Scalable;
    using King.Service.Data;
    
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
            var scalableQueue = new StorageQueue(config.ScalableQueueName, config.ConnectionString);
            yield return new InitializeStorageTask(scalableQueue);
            var dynamicQueue = new StorageQueue(config.DynamicQueueName, config.ConnectionString);
            yield return new InitializeStorageTask(dynamicQueue);

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
            yield return new BackoffRunner(new CompanyDequeuer(queue));

            //Dequeue task, Adaptive behavior
            yield return new AdaptiveRunner(new CompanyDequeuer(queue));

            //Dequeue task, Recurring behavior
            yield return new RecurringRunner(new CompanyDequeuer(queue));

            //Auto Scaling Task
            yield return new DynamicScaler(config);

            //Auto Scaling Dequeue Task
            yield return new ScalableQueue(scalableQueue, config);

            //Auto Batch Size Dequeue Task
            yield return new AdaptiveRunner(new StorageDequeueBatchDynamic<CompanyModel>(config.DynamicQueueName, config.ConnectionString, new CompanyProcessor()));

            //Dynamic Batch Size, Frequency, Threads (and queue creation)
            var f = new DequeueFactory(config.ConnectionString);
            
            foreach (var t in f.Tasks(config.FactoryQueueName, () => { return new CompanyProcessor(); }, QueuePriority.Medium))
            {
                yield return t;
            }
        }
    }
}
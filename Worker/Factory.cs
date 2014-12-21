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
    public class Factory : EasyTaskFactory<Configuration>
    {
        public override IEnumerable<IRunnable> Tasks(Configuration config)
        {
            // Initialization task
            yield return new InitTask();

            // Initialize Table; creates table if it doesn't already exist
            var table = new TableStorage(config.TableName, config.ConnectionString);
            yield return base.InitializeStorage(table);

            // Initialize Queue; creates queue if it doesn't already exist
            var queue = new StorageQueue(config.QueueName, config.ConnectionString);
            yield return base.InitializeStorage(queue);
            var scalableQueue = new StorageQueue(config.ScalableQueueName, config.ConnectionString);
            yield return base.InitializeStorage(scalableQueue);
            var dynamicQueue = new StorageQueue(config.DynamicQueueName, config.ConnectionString);
            yield return base.InitializeStorage(dynamicQueue);

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

            //Task once daily on the (specified/current) hour
            yield return new OnceDaily(config.ConnectionString);

            //Backoff task
            yield return new Backoff();

            //Self governing task
            yield return new Adaptive();

            //Dequeue task, Backoff behavior
            yield return base.Backoff(new CompanyDequeuer(queue));

            //Dequeue task, Adaptive behavior
            yield return base.Adaptive(new CompanyDequeuer(queue));

            //Dequeue task, Recurring behavior
            yield return base.Recurring(new CompanyDequeuer(queue));

            //Tasks for Queuing
            yield return new CompanyQueuer(queue);
            yield return new CompanyQueuer(scalableQueue);
            yield return new CompanyQueuer(dynamicQueue);

            //Auto Scaling Task
            yield return new DynamicScaler(config);

            //Auto Scaling Dequeue Task
            yield return new ScalableQueue(scalableQueue, config);

            //Auto Batch Size Dequeue Task
            yield return base.Adaptive(new StorageDequeueBatchDynamic<CompanyModel>(config.DynamicQueueName, config.ConnectionString, new CompanyProcessor()));

            //Dynamic Batch Size, Frequency, Threads (and queue creation)
            var f = new StorageDequeueFactory<CompanyModel>();
            var setup = new SetupExample()
            {
                ConnectionString = config.ConnectionString,
                Name = config.FactoryQueueName,
                Priority = QueuePriority.Medium,
            };
            
            foreach (var t in f.Tasks(setup))
            {
                yield return t;
            }
        }
    }
}
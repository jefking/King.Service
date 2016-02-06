namespace King.Service.CloudService.Role
{
    using System.Collections.Generic;
    using King.Azure.Data;
    using King.Service;
    using King.Service.CloudService.Role.Queue;

    //Tasks for Queuing (Demo purposes)
    public class DataGenerationFactory : ITaskFactory<Configuration>
    {
        public IEnumerable<IRunnable> Tasks(Configuration config)
        {
            //Tasks for Queuing (Demo purposes)
            yield return new CompanyQueuer(new StorageQueue(config.QueueName, config.ConnectionString));
            yield return new CompanyQueuer(new StorageQueue(config.ScalableQueueName, config.ConnectionString));
            yield return new CompanyQueuer(new StorageQueue(config.DynamicQueueName, config.ConnectionString));
            yield return new CompanyQueuer(new StorageQueue(config.FactoryQueueName, config.ConnectionString));
        }
    }
}
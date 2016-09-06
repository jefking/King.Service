namespace King.Service.CloudService.Role
{
    using King.Service;
    using King.Service.CloudService.Role.Queue;
    using System.Collections.Generic;

    //Tasks for Queuing (Demo purposes)
    public class DataGenerationFactory : ITaskFactory<Configuration>
    {
        public IEnumerable<IRunnable> Tasks(Configuration config)
        {
            //Tasks for Queuing (Demo purposes)
            return new IRunnable[] {
                new CompanyQueuer(config.GenericQueueName, config.ConnectionString),
                new CompanyQueuer(config.FastQueueName, config.ConnectionString),
                new CompanyQueuer(config.ModerateQueueName, config.ConnectionString),
                new CompanyQueuer(config.SlowQueueName, config.ConnectionString),
                new CompanyQueuerSharded(config.ShardQueueName, config.ConnectionString)
            };
        }
    }
}
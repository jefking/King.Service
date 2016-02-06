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
            return new IRunnable[] {
                new CompanyQueuer(config.GenericQueueName, config.ConnectionString),
                new CompanyQueuer(config.FastQueueName, config.ConnectionString),
                new CompanyQueuer(config.ModerateQueueName, config.ConnectionString),
                new CompanyQueuer(config.SlowQueueName, config.ConnectionString)
            };
        }
    }
}
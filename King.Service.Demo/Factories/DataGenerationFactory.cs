namespace King.Service.Demo.Factories
{
    using King.Service;
    using King.Service.Demo.Tasks;
    using System.Collections.Generic;

    //Tasks for Queuing (Demo purposes)
    public class DataGenerationFactory : ITaskFactory<AppConfig>
    {
        public IEnumerable<IRunnable> Tasks(AppConfig config)
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
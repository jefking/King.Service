namespace King.Service.CloudService.Role.Scalable
{
    using System.Collections.Generic;
    using King.Azure.Data;
    using King.Service;
    using King.Service.CloudService.Role.Queue;
    using King.Service.Data;

    public class ScalableQueue : QueueAutoScaler<Configuration>
    {
        public ScalableQueue(IQueueCount count, Configuration config)
            : base(count, 1, config, 1, 15, 1)
        {
        }

        public override IEnumerable<IScalable> ScaleUnit(Configuration config)
        {
            yield return new BackoffRunner(new CompanyDequeuer(new StorageQueue(config.ScalableQueueName, config.ConnectionString)));
        }
    }
}
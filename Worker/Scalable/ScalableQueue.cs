namespace Worker.Scalable
{
    using King.Azure.Data;
    using King.Service;
    using King.Service.Data;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Worker.Queue;

    public class ScalableQueue : QueueAutoScaler<Configuration>
    {
        public ScalableQueue(IQueueCount count, Configuration config)
            : base(count, config, 1, 15, 1)
        {
        }

        public override IEnumerable<IScalable> ScaleUnit(Configuration config)
        {
            yield return new BackoffRunner(new CompanyDequeuer(new StorageQueue(config.ScalableQueueName, config.ConnectionString)));
        }
    }
}
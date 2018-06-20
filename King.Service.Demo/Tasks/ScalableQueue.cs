namespace King.Service.Demo.Tasks
{
    using global::Azure.Data.Wrappers;
    using King.Service;
    using King.Service.Data;
    using System.Collections.Generic;

    public class ScalableQueue : QueueAutoScaler<AppConfig>
    {
        public ScalableQueue(IQueueCount count, AppConfig config)
            : base(count, 1, config, 1, 15, 1)
        {
        }

        public override IEnumerable<IScalable> ScaleUnit(AppConfig config)
        {
            yield return new BackoffRunner(new CompanyDequeuer(config.FastQueueName, config.ConnectionString));
        }
    }
}
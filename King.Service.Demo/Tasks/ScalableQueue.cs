namespace King.Service.Demo.Tasks
{
    using King.Service;
    using King.Service.Data;
    using System.Collections.Generic;

    /// <summary>
    /// Scales dequeue tasks threads based on queue load
    /// </summary>
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
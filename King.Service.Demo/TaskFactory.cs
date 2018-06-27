namespace King.Service.Demo
{
    using King.Service.Demo.Tasks;
    using System.Collections.Generic;

    public class TaskFactory : ITaskFactory<AppConfig>
    {
        public IEnumerable<IRunnable> Tasks(AppConfig config)
        {
            // Task with adaptive timing
            yield return new Adaptive();
            // Task with backoff timing
            yield return new Backoff();
            // Task that happens with conisistent frequency
            yield return new Recurring();

            // Initialize task, occurs once at start-up
            yield return new InitTask();

            // Occurs on a timeframe, across compute nodes
            yield return new Coordinated(config.ConnectionString);
            // Occurs on once daily, across compute nodes
            yield return new OnceDaily(config.ConnectionString);

            // Queues messages to sharded queue
            yield return new CompanyQueuerSharded(config.GenericQueueName, config.ConnectionString);
            // Queues messages
            yield return new CompanyQueuer(config.GenericQueueName, config.ConnectionString);
        }
    }
}
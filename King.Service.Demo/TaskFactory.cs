namespace King.Service.Demo
{
    using King.Service.Demo.Tasks;
    using System.Collections.Generic;

    public class TaskFactory : ITaskFactory<AppConfig>
    {
        public IEnumerable<IRunnable> Tasks(AppConfig config)
        {
            yield return new Adaptive();
            yield return new Backoff();
            yield return new Recurring();

            yield return new InitTask();

            yield return new Coordinated(config.ConnectionString);
            yield return new OnceDaily(config.ConnectionString);

            yield return new CompanyQueuer(config.GenericQueueName, config.ConnectionString);
        }
    }
}
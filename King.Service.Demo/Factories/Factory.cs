namespace King.Service.Demo.Factories
{
    using King.Service;
    using King.Service.Demo.Tasks;
    using System.Collections.Generic;

    /// <summary>
    /// Builds manifest of tasks to run
    /// </summary>
    public class Factory : ITaskFactory<AppConfig>
    {
        public IEnumerable<IRunnable> Tasks(AppConfig config)
        {
            // List of tasks to run
            return new List<IRunnable>(new IRunnable[] {
                // Initialization task
                new InitTask(),

                //basic task
                new Recurring(),

                //Backoff task
                new Backoff(),

                //Self governing task
                new Adaptive(),
            });
        }
    }
}
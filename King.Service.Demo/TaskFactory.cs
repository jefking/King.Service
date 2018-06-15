namespace King.Service.Demo
{
    using System.Collections.Generic;

    public class TaskFactory : ITaskFactory<Configuration>
    {
        public IEnumerable<IRunnable> Tasks(Configuration passthrough)
        {
            yield return null;
        }
    }
}
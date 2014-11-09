namespace Worker
{
    using King.Azure.Data;
    using King.Service;
    using King.Service.Data;
    using King.Service.WorkerRole;
    using System.Collections.Generic;
    using Worker.Queue;
    using Worker.Scalable;

    /// <summary>
    /// Task Factory
    /// </summary>
    public class Factory : ITaskFactory<Configuration>
    {
        public IEnumerable<IRunnable> Tasks(Configuration config)
        {

            //Backoff task(s)
            yield return new Backoff();

            //Self Governing task(s)
             yield return new Adaptive();
        }
    }
}
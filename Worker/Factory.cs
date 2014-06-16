namespace Worker
{
    using King.Azure.BackgroundWorker;
    using System.Collections.Generic;

    public class Factory : ServiceFactory
    {
        public override IEnumerable<IRunnable> Services(object passthrough)
        {
            var services = new List<IRunnable>();
            services.Add(new Task());
            services.Add(new SynchronizedTask());

            //Add More Tasks

            return services;
        }
    }
}
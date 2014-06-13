namespace Worker
{
    using King.Azure.BackgroundWorker;
    using System;
    using System.Collections.Generic;

    public class Factory : ServiceFactory
    {
        public override IEnumerable<IRunnable> Services(object passthrough)
        {
            var services = new List<IRunnable>();
            services.Add(new Task());
            //Add More Tasks
            return services;
        }
    }
}

namespace Worker
{
    using King.Azure.BackgroundWorker;
    using King.Azure.BackgroundWorker.Data;
    using System.Collections.Generic;

    public class Factory : ServiceFactory
    {
        public override IEnumerable<IRunnable> Services(object passthrough)
        {
            var services = new List<IRunnable>();
            
            //Independant work
            services.Add(new Task());

            //Work coordinated between servers
            var task = new CoordinatedTask();
            // Add once to ensure that Table is created for Instances to communicate with
            services.Add(task.InitializeTask());
            // Add your coordinated task(s)
            services.Add(task);
            
            return services;
        }
    }
}
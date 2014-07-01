namespace Worker
{
    using King.Azure.BackgroundWorker;
    using System.Collections.Generic;

    public class Factory : ServiceFactory
    {
        public override IEnumerable<IRunnable> Services(object passthrough)
        {
            var services = new List<IRunnable>();
            // Initialization Task(s)
            services.Add(new InitTask());

            //Task(s)
            services.Add(new Task());

            //Cordinated Tasks between Instances

            var task = new Coordinated();
            // Add once to ensure that Table is created for Instances to communicate with
            services.Add(task.InitializeTask());

            // Add your coordinated task(s)
            services.Add(task);
            
            return services;
        }
    }
}
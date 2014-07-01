namespace Worker
{
    using King.Azure.BackgroundWorker;
    using System.Collections.Generic;

    public class Factory : TaskFactory
    {
        public override IEnumerable<IRunnable> Tasks(object passthrough)
        {
            var tasks = new List<IRunnable>();
            // Initialization Task(s)
            tasks.Add(new InitTask());

            //Task(s)
            tasks.Add(new Task());

            //Cordinated Tasks between Instances

            var task = new Coordinated();
            // Add once to ensure that Table is created for Instances to communicate with
            tasks.Add(task.InitializeTask());

            // Add your coordinated task(s)
            tasks.Add(task);
            
            return tasks;
        }
    }
}
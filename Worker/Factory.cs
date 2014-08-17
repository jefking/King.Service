namespace Worker
{
    using King.Service;
    using King.Service.Data;
    using System.Collections.Generic;
    using Worker.Queue;

    public class Factory : TaskFactory
    {
        public override IEnumerable<IRunnable> Tasks(object passthrough)
        {
            //Load configuration to pass to objects
            var connectionString = "UseDevelopmentStorage=true;";

            //Build a manifest of Tasks to run
            var tasks = new List<IRunnable>();

            // Initialization Task(s)
            tasks.Add(new InitTask());

            // Initialize Table; creates table if it doesn't already exist
            var table = new TableStorage("table", connectionString);
            tasks.Add(new InitializeStorageTask(table));

            // Initialize Queue; creates queue if it doesn't already exist
            var queue = new StorageQueue("queue", connectionString);
            tasks.Add(new InitializeStorageTask(queue));

            // Initialize Queue; creates queue if it doesn't already exist
            var container = new Container("container", connectionString);
            tasks.Add(new InitializeStorageTask(container));

            //Task(s)
            tasks.Add(new Task());

            //Cordinated Tasks between Instances
            var task = new Coordinated(connectionString);

            // Add once to ensure that Table is created for Instances to communicate with
            tasks.Add(task.InitializeTask());

            // Add your coordinated task(s)
            tasks.Add(task);

            //Backoff task(s)
            tasks.Add(new Backoff());

            //Self Governing task(s)
            tasks.Add(new Adaptive());

            //Dequeue task(s)
            tasks.Add(new BackoffRunner(new CompanyDequeuer(connectionString)));

            //Task for Queuing
            tasks.Add(new CompanyQueuer());

            return tasks;
        }
    }
}
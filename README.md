Task scheduling for Azure; use with worker roles for running background processing tasks.
- Create a task to initialize your environment
- Create a task that runs every X amount of time per instance
- Create a task that runs every X amount of time across any number of instances
- [Example Project](https://github.com/jefking/King.Azure.BackgroundWorker/tree/master/Worker)

## NuGet
[Add via NuGet](https://www.nuget.org/packages/King.Service)
```
PM> Install-Package King.Service
```

## Sample Code
### [Initialization Task](https://github.com/jefking/King.Azure.BackgroundWorker/blob/master/Worker/InitTask.cs)
```
class MyTask : InitializeTask
{
	public override void Run()
	{
		//Initialize one time resource.
	}
}
```
### [Repetitive Task](https://github.com/jefking/King.Azure.BackgroundWorker/blob/master/Worker/Task.cs)
```
class MyTask : TaskManager
{
	public override void Run()
	{
		//Process background work here.
	}
}
```
### [Backoff Task](https://github.com/jefking/King.Azure.BackgroundWorker/blob/master/Worker/Backoff.cs)
```
class MyTask : BackoffTask
{
	public override void Run(out bool workWasDone)
	{
		//Process background work here.
	}
}
```
### [Coordinated Task](https://github.com/jefking/King.Azure.BackgroundWorker/blob/master/Worker/Coordinated.cs)
```
class MyTask : CoordinatedTask
{
	public MyTask()
		: base("Storage Account for Coordination between Instances")
	{
	}
	public override void Run()
	{
		//Process background work here.
	}
}
```
### [Initialize Tasks](https://github.com/jefking/King.Azure.BackgroundWorker/blob/master/Worker/Factory.cs)
```
class Factory : TaskFactory
{
    public override IEnumerable<IRunnable> Tasks(object passthrough)
    {
        var tasks = new List<IRunnable>();
        // Initialization Task(s)
        tasks.Add(new InitTask());

        // Initialize Table; creates table if it doesn't already exist
        var table = new TableStorage("table", "UseDevelopmentStorage=true;");
        tasks.Add(new InitializeStorageTask(table));

        // Initialize Queue; creates queue if it doesn't already exist
        var queue = new Queue("queue", "UseDevelopmentStorage=true;");
        tasks.Add(new InitializeStorageTask(queue));
		
        // Initialize Queue; creates queue if it doesn't already exist
        var container = new Container("container", "UseDevelopmentStorage=true;");
        tasks.Add(new InitializeStorageTask(container));

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
```
### [Worker Role](https://github.com/jefking/King.Azure.BackgroundWorker/blob/master/Worker/WorkerRole.cs)
```
public class WorkerRole : RoleEntryPoint
{
    private RoleTaskManager manager = new RoleTaskManager(new Factory());

    public override void Run()
    {
        this.manager.Run();

        base.Run();
    }

    public override bool OnStart()
    {
        return this.manager.OnStart();
    }

    public override void OnStop()
    {
        this.manager.OnStop();

        base.OnStop();
    }
}
```
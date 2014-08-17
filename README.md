Task scheduling for Azure; use with worker roles for running background data processing tasks.
- Create a task to initialize your environment
- Create a task that runs every X amount of time per instance
- Create a task that runs every X; and when work lessens backs off to Y
- Create a task that determines the needed rate via frequency of processing tasks
- Create a task that runs every X amount of time across any number of instances
- And much more! [View Wiki](https://github.com/jefking/King.Azure.BackgroundWorker/wiki)
- [Example Project](https://github.com/jefking/King.Azure.BackgroundWorker/tree/master/Worker)

## NuGet
[Add via NuGet](https://www.nuget.org/packages/King.Service)
```
PM> Install-Package King.Service
```

## Get Started
### [Initialize Tasks](https://github.com/jefking/King.Azure.BackgroundWorker/blob/master/Worker/Factory.cs)
```
class Factory : TaskFactory
{
    public override IEnumerable<IRunnable> Tasks(object passthrough)
    {
        var tasks = new List<IRunnable>();
        // Initialization Task(s)
        tasks.Add(new InitTask());

        //Task(s)
        tasks.Add(new Task());

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

#### [More on the Wiki](https://github.com/jefking/King.Azure.BackgroundWorker/wiki)
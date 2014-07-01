King.Azure.BackgroundWorker
============

Simple Service Scheduling for Azure; meant to be used in background worker roles.
- Create a task that runs every X amount of time per instance
- Create a task that runs every X amount of time across any number of instances

## NuGet
[Add via NuGet](https://www.nuget.org/packages/King.Service)
```
PM> Install-Package King.Service
```
## Examples
### Implement Repetitive Task
```
class MyTask : TaskManager
{
	public MyTask()
		: base(15, 60)
	{
	}
	public virtual void Run()
	{
		//Process background work here.
	}
}
```
### Implement Coordinated Task
```
class MyTask : CoordinatedTask
{
	public MyTask()
		: base("Storage Account for Coordination between Instances", 60)
	{
	}
	public virtual void Run()
	{
		//Process background work here.
	}
}
```
### Initialize Services
```
class Factory : ServiceFactory
{
    public override IEnumerable<IRunnable> Services(object passthrough)
    {
        var services = new List<IRunnable>();
        // Initialization Services
        services.Add(new InitializeTask());

        //Tasks
        services.Add(new Task());

        //Cordinated Tasks between Instances

        var task = new Coordinated();
        // Add once to ensure that Table is created for Instances to communicate with
        services.Add(task.InitializeTask());

        // Add your Coordinated task(s)
        services.Add(task);
            
        return services;
    }
}
```
## Demo Project
[Azure Project](https://github.com/jefking/King.Azure.BackgroundWorker/tree/master/Azure.Demo)

[Worker Role](https://github.com/jefking/King.Azure.BackgroundWorker/tree/master/Worker)

## Contributing

Contributions are always welcome. If you have find any issues, please report them to the [Github Issues Tracker](https://github.com/jefking/King.Azure.BackgroundWorker/issues?sort=created&direction=desc&state=open).

## Apache 2.0 License

Copyright 2014 Jef King

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

[http://www.apache.org/licenses/LICENSE-2.0](http://www.apache.org/licenses/LICENSE-2.0)

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
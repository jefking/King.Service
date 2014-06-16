King.Azure.BackgroundWorker
============

Simple Service Scheduling for Azure; meant to be used in background worker roles.
- Create a task that runs every X amount of time per instance
- Create a task that runs every X amount of time across instances

## NuGet
[Add via NuGet](https://www.nuget.org/packages/King.Service)
```
PM> Install-Package King.Service
```
## Examples
### Initialize ^(Used only by CoordinatedManager)^
```
var connectionString = Configuration["Key"];
var core = new ScheduledTaskCore(TimeSpan.FromHours(100), connectionString);
core.InitializeTable();
```
### Implement Repetitive Task
```
class MyTask : Manager
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
class MyTask : CoordinatedManager
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
## About the Author

Jef King has worked in the software industry for fourteen years. Over this time he has experienced a range of responsibilities in various industries. His passion for technology and motivating teams has kept his drive and focus strong. Early on in his career he showed an entrepreneurial spirit, starting multiple small companies. He departed from this to learn more about the software industry by working with larger companies, such as Microsoft. These diverse experiences have given a very unique perspective on teams and software engineering. Since moving back to Vancouver he has built several highly productive software development teams, and inspired others to try similar techniques.

## Contributing

Contributions are always welcome. If you have find any issues, please report them to the [Github Issues Tracker](https://github.com/jefkingabc/King.Service/issues?sort=created&direction=desc&state=open).

## Apache 2.0 Licence

Copyright 2014 Jef King

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

[http://www.apache.org/licenses/LICENSE-2.0](http://www.apache.org/licenses/LICENSE-2.0)

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
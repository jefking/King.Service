### Task scheduling for Azure and Windows
- Create a task to initialize your environment
- Plugs into Azure Worker Roles
- Initialization: Azure Storage Queues, Tables, Containers
- Run tasks such as WFC services
- Create a task that runs every X amount of time per instance
- Create a task that runs every X; and when work lessens backs off to Y
- Create a task that determines the needed rate via frequency of processing tasks
- Create a task that occurs once when running on many machines
- Create a task that runs at a specified hour (and minute) on one machine daily
- Dequeue messages and batches of messages for Azure Storage Queues
- And much more! [View Wiki](https://github.com/jefking/King.Service/wiki)
- [Example Project](https://github.com/jefking/King.Service/tree/master/Worker)

### [NuGet](https://www.nuget.org/packages/King.Service)
```
PM> Install-Package King.Service
```

### [Wiki](https://github.com/jefking/King.Service/wiki)
View the wiki to learn how to use this.
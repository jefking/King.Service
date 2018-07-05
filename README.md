### Task scheduling for .NET
1. Auto-Scale microservices within your compute
2. Tasks to initialize your resources
 + Create Azure Storage: Queues, Tables and Containers
 + Load WCF services
3. Create Tasks that Occur:
 + Every X seconds per server instance
 + Every X seconds; and lessens frequency to Y when there is no work
 + That determines the needed rate via frequency of processing tasks
 + Once, even with multiple servers
 + Runs at a specified time (resolution to the hour, or the minute) on one server
4. Dequeue from Azure Storage Queues
 + Batches of messages
 + Shards for high throughput
 + Variable timing for cost saving
5. Extension for working with: [Service Bus](https://github.com/jefking/King.Service.ServiceBus)

### [NuGet](https://www.nuget.org/packages/King.Service)
```
PM> Install-Package King.Service
```

### [Wiki](https://github.com/jefking/King.Service/wiki)
View the wiki to learn how to use this.

### CI
![Build Status](https://threadmaster.visualstudio.com/f68f61db-3709-453e-9eb8-8297d793734b/_apis/build/status/8)
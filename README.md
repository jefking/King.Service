# Task scheduling for .NET
1. Auto-Scale microservices within your compute
2. Tasks to initialize your resources
 + Create Azure Storage: Queues, Tables and Containers
 + Load WCF services
3. Create Tasks that Occur:
 + Every X seconds per server instance
 + Every X seconds; lessens frequency to Y when there is limited work
 + That determines the needed rate via frequency of processing tasks
 + Once, even with multiple servers
 + Runs at a specified time (resolution to the hour, or the minute) on one server
4. Dequeue from Azure Storage Queues
 + Batches of messages
 + Shards for high throughput
 + Variable timing for cost savings
5. Extension for working with: [Service Bus](https://github.com/jefking/King.Service.ServiceBus)

# Ready, Set, Go!
## [NuGet](https://www.nuget.org/packages/King.Service)
```
PM> Install-Package King.Service
```

## [Demo Container](https://hub.docker.com/r/jefking/king.service.demo)
Create Azure Storage Account; Blob + Queue

### Pull
```
docker pull jefking/king.service.demo
```

### Run
```
docker run -it jefking/king.service.demo <YOUR STORAGE ACCOUNT CONNECTION>
```

## CI
[![Build status](https://github.com/jefking/King.Service/actions)]

## [Docs](https://github.com/jefking/King.Service/wiki)
View the wiki to learn how to use this.
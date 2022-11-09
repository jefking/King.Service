# Task scheduling for .NET core
[![.NET](https://github.com/jefking/King.Service/actions/workflows/dotnet.yml/badge.svg)](https://github.com/jefking/King.Service/actions/workflows/dotnet.yml)
1. Auto-Scale micro-services within your compute
2. Tasks to initialize your resources
 + Load WCF services
3. Create Tasks that Occur:
 + Every X seconds per server instance
 + Every X seconds; lessens frequency to Y when there is limited work
 + That determines the needed rate via frequency of processing tasks
 + Once, even with multiple servers
 + Runs at a specified time (resolution to the hour, or the minute) on one server
4. Extension for working with: [Azure Storage](https://github.com/jefking/King.Service.Azure)
5. Extension for working with: [Service Bus](https://github.com/jefking/King.Service.ServiceBus)

# Ready, Set, Go!
## [NuGet](https://www.nuget.org/packages/King.Service)
```
PM> Install-Package King.Service
```

## [Docs](https://github.com/jefking/King.Service/wiki)
View the [wiki](https://github.com/jefking/King.Service/wiki) to learn how to use this.
namespace King.Service.ServiceBus
{
    using King.Service.Data;
    using King.Service.ServiceBus.Queue;
    using King.Service.WorkerRole.Queue;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using System.Collections.Generic;

    public class Factory : TaskFactory
    {
        public override IEnumerable<IRunnable> Tasks(object passthrough)
        {
            var tasks = new List<IRunnable>();

            //Initialization
            tasks.Add(new InitializeQueues());

            //Connection
            var pollingClient = QueueClient.Create("polling");
            var eventClient = QueueClient.Create("events");

            //Dequeuing; generic for re-use across your code base (just change the model)
            var poller = new ServiceBusQueuePoller<ExampleModel>(pollingClient);

            //Specific processor for ExampleModel.
            var processor = new ExampleProcessor();

            //Load polling dequeue object to run
            var dequeue = new Dequeue<ExampleModel>(poller, processor);

            //Task Manifest for King.Service
            tasks.Add(new BackoffRunner(dequeue));

            //Task for watching for queue events
            tasks.Add(new Events(eventClient));

            //Tasks for queuing work
            tasks.Add(new QueueForPoll(pollingClient));
            tasks.Add(new QueueForEvents(eventClient));

            return tasks;
        }
    }
}

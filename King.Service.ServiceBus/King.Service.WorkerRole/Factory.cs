namespace King.Service.ServiceBus
{
    using King.Service.Data;
    using King.Service.ServiceBus.Queue;
    using King.Service.WorkerRole;
    using King.Service.WorkerRole.Queue;
    using Microsoft.ServiceBus.Messaging;
    using System.Collections.Generic;

    /// <summary>
    /// Facotry
    /// </summary>
    public class Factory : ITaskFactory<Configuration>
    {
        /// <summary>
        /// Load Tasks
        /// </summary>
        /// <param name="passthrough"></param>
        /// <returns></returns>
        public IEnumerable<IRunnable> Tasks(Configuration config)
        {
            //Initialization
            yield return new InitializeQueues();

            //Connection
            var pollingClient = QueueClient.Create(config.PollingName);
            var eventClient = QueueClient.Create(config.EventsName);

            //Dequeuing; generic for re-use across your code base (just change the model)
            var poller = new ServiceBusQueuePoller<ExampleModel>(pollingClient);

            //Specific processor for ExampleModel.
            var processor = new ExampleProcessor();

            //Load polling dequeue object to run
            var dequeue = new Dequeue<ExampleModel>(poller, processor);

            //Task Manifest for King.Service
            yield return new BackoffRunner(dequeue);

            //Task for watching for queue events
            yield return new Events(eventClient);

            //Tasks for queuing work
            yield return new QueueForPoll(pollingClient);
            yield return new QueueForEvents(eventClient);
        }
    }
}

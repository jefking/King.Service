namespace King.Service.WorkerRole.Queue
{
    using Microsoft.ServiceBus;


    public class InitializeQueues : InitializeTask
    {
        public override void Run()
        {
            var manager = NamespaceManager.Create();
            if (!manager.QueueExists("polling"))
            {
                manager.CreateQueue("polling");
            }
            if (!manager.QueueExists("events"))
            {
                manager.CreateQueue("events");
            }
        }
    }
}
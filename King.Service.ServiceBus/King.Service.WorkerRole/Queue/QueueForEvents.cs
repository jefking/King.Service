namespace King.Service.WorkerRole.Queue
{
    using King.Service.ServiceBus.Queue;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Diagnostics;

    public class QueueForEvents : RecurringTask
    {
        #region Members
        /// <summary>
        /// Queue Client
        /// </summary>
        private readonly QueueClient client;
        #endregion

        #region Constructors
        public QueueForEvents(QueueClient client)
        {
            if (null == client)
            {
                throw new ArgumentNullException("client");
            }

            this.client = client;
        }
        #endregion

        public override void Run()
        {
            var model = new ExampleModel()
            {
                Identifier = Guid.NewGuid(),
                Name = "Message Event",
            };

            Trace.TraceInformation("Sending to Event: '{0}/{1}'", model.Name, model.Identifier);

            client.Send(new BrokeredMessage(model));
        }
    }
}
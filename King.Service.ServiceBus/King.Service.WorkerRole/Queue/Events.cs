namespace King.Service.WorkerRole.Queue
{
    using King.Service.ServiceBus.Queue;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Diagnostics;

    public class Events : InitializeTask
    {
        #region Members
        /// <summary>
        /// Queue Client
        /// </summary>
        private readonly QueueClient client;
        #endregion

        #region Constructors
        public Events(QueueClient client)
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
            // Build the messaging options.
            var eventDrivenMessagingOptions = new OnMessageOptions()
            {
                AutoComplete = true,
                MaxConcurrentCalls = 5,
            };

            eventDrivenMessagingOptions.ExceptionReceived += OnExceptionReceived;

            // Subscribe for messages.
            this.client.OnMessage(OnMessageArrived, eventDrivenMessagingOptions);
        }

        /// <summary>
        /// This event will be called each time a message arrives.
        /// </summary>
        /// <param name="message"></param>
        public void OnMessageArrived(BrokeredMessage message)
        {
            var data = message.GetBody<ExampleModel>();
            Trace.TraceInformation("Recieved Event: '{0}/{1}'", data.Name, data.Identifier);
        }

        /// <summary>
        /// Event handler for each time an error occurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnExceptionReceived(object sender, ExceptionReceivedEventArgs e)
        {
            if (e != null && e.Exception != null)
            {
                Trace.TraceError(e.Exception.ToString());
            }
        }
    }
}
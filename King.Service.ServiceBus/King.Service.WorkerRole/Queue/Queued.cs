namespace King.Service.ServiceBus.Queue
{
    using King.Azure.Data;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Generic Wrapper for Brokered Messages
    /// </summary>
    /// <typeparam name="T">Type, Serialized in Message Body</typeparam>
    public class Queued<T> : IQueued<T>
    {
        private readonly BrokeredMessage msg = null;

        public Queued(BrokeredMessage msg)
        {
            if (null == msg)
            {
                throw new ArgumentNullException("msg");
            }

            this.msg = msg;
        }

        public async Task Abandon()
        {
            await msg.AbandonAsync();
        }

        public async Task<T> Data()
        {
            return await Task.Factory.StartNew(() =>
                {
                    return msg.GetBody<T>();
                });
        }

        public async Task Complete()
        {
            await this.msg.CompleteAsync();
        }

        public async Task Delete()
        {
            await this.msg.CompleteAsync();
        }
    }
}
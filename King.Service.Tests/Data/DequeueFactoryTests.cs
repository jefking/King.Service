namespace King.Service.Tests.Data
{
    using System;
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Scalability;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DequeueFactoryTests
    {
        const string ConnectionString = "UseDevelopmentStorage=true";

        class MyFactory : DequeueFactory<string>
        {
            public MyFactory(IQueueThroughput throughput = null)
                : base(throughput)
            {
            }

            public override IRunnable Scalable(string queueName, IProcessor<string> processor, QueuePriority priority = QueuePriority.Low)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void Constructor()
        {
            new MyFactory();
        }

        [Test]
        public void ConstructorThroughputNull()
        {
            new MyFactory(null);
        }
    }
}
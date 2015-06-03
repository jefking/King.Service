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

        [Test]
        public void Constructor()
        {
            new DequeueFactory<object>();
        }

        [Test]
        public void ConstructorThroughputNull()
        {
            new DequeueFactory<object>(null);
        }

        [Test]
        public void Runnable()
        {
            var queue = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();

            var df = new DequeueFactory<object>();
            var t = df.Runnable(queue, ConnectionString, processor);

            Assert.IsNotNull(t);
            Assert.IsNotNull(t as QueueSimplifiedScaler);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RunnableQueueNull()
        {
            var processor = Substitute.For<IProcessor<object>>();

            var df = new DequeueFactory<object>();
            var t = df.Runnable(null, ConnectionString, processor);

            Assert.IsNotNull(t);
            Assert.IsNotNull(t as QueueSimplifiedScaler);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RunnableConnectionNull()
        {
            var queue = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();

            var df = new DequeueFactory<object>();
            var t = df.Runnable(queue, null, processor);

            Assert.IsNotNull(t);
            Assert.IsNotNull(t as QueueSimplifiedScaler);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RunnableProcessorNull()
        {
            var queue = Guid.NewGuid().ToString();

            var df = new DequeueFactory<object>();
            var t = df.Runnable(queue, ConnectionString, null);

            Assert.IsNotNull(t);
            Assert.IsNotNull(t as QueueSimplifiedScaler);
        }
    }
}
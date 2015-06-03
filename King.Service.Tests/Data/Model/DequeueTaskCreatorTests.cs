namespace King.Service.Tests.Data.Model
{
    using System;
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Data.Model;
    using King.Service.Scalability;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DequeueTaskCreatorTests
    {
        const string ConnectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            var queue = Guid.NewGuid().ToString();
            var connection = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();
            new DequeueTaskCreator<object>(queue, connection, processor);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorQueueNameNull()
        {
            var connection = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();
            new DequeueTaskCreator<object>(null, connection, processor);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorConnectionNull()
        {
            var queue = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();
            new DequeueTaskCreator<object>(queue, null, processor);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorProcessorNull()
        {
            var queue = Guid.NewGuid().ToString();
            var connection = Guid.NewGuid().ToString();
            new DequeueTaskCreator<object>(queue, connection, null);
        }

        [Test]
        public void IsITaskCreator()
        {
            var queue = Guid.NewGuid().ToString();
            var connection = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();
            Assert.IsNotNull(new DequeueTaskCreator<object>(queue, connection, processor) as ITaskCreator);
        }

        [Test]
        public void Task()
        {
            var queue = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();
            var throughput = Substitute.For<IQueueThroughput>();
            throughput.Runner(Arg.Any<IDynamicRuns>(), QueuePriority.High);

            var dtc = new DequeueTaskCreator<object>(queue, ConnectionString, processor, QueuePriority.High, throughput);

            var t = dtc.Task;
            Assert.IsNotNull(t);
            var r = t();
            Assert.IsNotNull(r);

            throughput.Received().Runner(Arg.Any<IDynamicRuns>(), QueuePriority.High);
        }
    }
}
namespace King.Service.Tests.Scalability
{
    using King.Azure.Data;
    using King.Service.Scalability;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class QueueScalerTests
    {
        [Test]
        public void Constructor()
        {
            var queue = Substitute.For<IQueueCount>();
            new QueueScaler<object>(queue);
        }

        [Test]
        public void IsScaler()
        {
            var queue = Substitute.For<IQueueCount>();
            Assert.IsNotNull(new QueueScaler<object>(queue) as Scaler<object>);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorQueueNull()
        {
            new QueueScaler<object>(null);
        }

        [Test]
        public void Maximum()
        {
            var random = new Random();
            var max = (ushort)random.Next(ushort.MinValue, ushort.MaxValue);
            var queue = Substitute.For<IQueueCount>();
            var qs = new QueueScaler<object>(queue, max);
            Assert.AreEqual(max, qs.Maximum);
        }

        [Test]
        public void MaximumMinimum()
        {
            var random = new Random();
            var max = (ushort)random.Next(ushort.MinValue, 9);
            var queue = Substitute.For<IQueueCount>();
            var qs = new QueueScaler<object>(queue, max);
            Assert.AreEqual(10, qs.Maximum);
        }

        [Test]
        public void ShouldScaleUp()
        {
            var queue = Substitute.For<IQueueCount>();
            queue.ApproixmateMessageCount().Returns(Task.FromResult<long?>(10000));

            var qs = new QueueScaler<object>(queue);
            var result = qs.ShouldScale();

            Assert.IsTrue(result.Value);

            queue.Received().ApproixmateMessageCount();
        }

        [Test]
        public void ShouldScaleDown()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var queue = Substitute.For<IQueueCount>();
            queue.ApproixmateMessageCount().Returns(Task.FromResult<long?>(1));

            var qs = new QueueScaler<object>(queue, 12);
            qs.ScaleUp(factory, new object(), Guid.NewGuid().ToString());
            qs.ScaleUp(factory, new object(), Guid.NewGuid().ToString());

            var result = qs.ShouldScale();

            Assert.IsFalse(result.Value);

            queue.Received().ApproixmateMessageCount();
        }

        [Test]
        public void ShouldScaleOptimal()
        {
            var random = new Random();
            var count = (ushort)random.Next(ushort.MinValue, ushort.MaxValue);

            var factory = Substitute.For<ITaskFactory<object>>();

            var queue = Substitute.For<IQueueCount>();
            queue.ApproixmateMessageCount().Returns(Task.FromResult<long?>(count));

            var qs = new QueueScaler<object>(queue, count);
            qs.ScaleUp(factory, new object(), Guid.NewGuid().ToString());

            var result = qs.ShouldScale();

            Assert.IsNull(result);

            queue.Received().ApproixmateMessageCount();
        }

        [Test]
        public void ShouldScaleThrows()
        {
            var queue = Substitute.For<IQueueCount>();
            queue.When(q => q.ApproixmateMessageCount()).Do(x => { throw new Exception(); });

            var qs = new QueueScaler<object>(queue);
            var result = qs.ShouldScale();

            Assert.IsFalse(result.HasValue);

            queue.Received().ApproixmateMessageCount();
        }
    }
}
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
            Assert.AreEqual(100, qs.Maximum);
        }

        [Test]
        public void ShouldScaleTrue()
        {
            var queue = Substitute.For<IQueueCount>();
            queue.ApproixmateMessageCount().Returns(Task.FromResult<long?>(10000));

            var qs = new QueueScaler<object>(queue);
            var result = qs.ShouldScale();

            Assert.IsTrue(result.Value);

            queue.Received().ApproixmateMessageCount();
        }

        [Test]
        public void ShouldScaleFalse()
        {
            var queue = Substitute.For<IQueueCount>();
            queue.ApproixmateMessageCount().Returns(Task.FromResult<long?>(1));

            var qs = new QueueScaler<object>(queue);
            var result = qs.ShouldScale();

            Assert.IsFalse(result.Value);

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
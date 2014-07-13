namespace King.Azure.BackgroundWorker.Tests
{
    using King.Azure.BackgroundWorker.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class DequeueTests
    {
        [Test]
        public void Constructor()
        {
            var processor = Substitute.For<IDequeueProcessor<object>>();
            new Dequeue<object>(processor);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinMaxZero()
        {
            var processor = Substitute.For<IDequeueProcessor<object>>();
            var random = new Random();
            new Dequeue<object>(processor, random.Next(0));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinMaxSwitched()
        {
            var processor = Substitute.For<IDequeueProcessor<object>>();
            var random = new Random();
            var value = random.Next(2, 1024);
            new Dequeue<object>(processor, value + 1, value - 1);
        }

        [Test]
        public void IsIBackoffRuns()
        {
            var processor = Substitute.For<IDequeueProcessor<object>>();
            var d = new Dequeue<object>(processor);
            Assert.IsNotNull(d as IBackoffRuns);
        }

        [Test]
        public async Task Run()
        {
            var processor = Substitute.For<IDequeueProcessor<object>>();
            processor.Poll().Returns(Task.FromResult((IQueued<object>)null));
            var d = new Dequeue<object>(processor);
            var result = await d.Run();
            Assert.IsFalse(result);
        }

        [Test]
        public void MinimumPeriodInSeconds()
        {
            var processor = Substitute.For<IDequeueProcessor<object>>();
            var d = new Dequeue<object>(processor);
            Assert.AreEqual(15, d.MinimumPeriodInSeconds);
        }

        [Test]
        public void MaximumPeriodInSeconds()
        {
            var processor = Substitute.For<IDequeueProcessor<object>>();
            var d = new Dequeue<object>(processor);
            Assert.AreEqual(300, d.MaximumPeriodInSeconds);
        }
    }
}
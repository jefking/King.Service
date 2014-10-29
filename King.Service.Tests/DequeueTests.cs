namespace King.Service.Tests
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Timing;
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
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            new Dequeue<object>(poller, processor);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorPollerNull()
        {
            var processor = Substitute.For<IProcessor<object>>();
            new Dequeue<object>(null, processor);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorProcessorNull()
        {
            var poller = Substitute.For<IPoller<object>>();
            new Dequeue<object>(poller, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinMaxZero()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();

            var random = new Random();
            new Dequeue<object>(poller, processor, random.Next(0));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinMaxSwitched()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();

            var random = new Random();
            var value = random.Next(2, 1024);
            new Dequeue<object>(poller, processor, value + 1, value - 1);
        }

        [Test]
        public void IsIBackoffRuns()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();

            var d = new Dequeue<object>(poller, processor);
            Assert.IsNotNull(d as IDynamicRuns);
        }

        [Test]
        public async Task Run()
        {
            var poller = Substitute.For<IPoller<object>>();
            poller.Poll().Returns(Task.FromResult<IQueued<object>>(null));

            var processor = Substitute.For<IProcessor<object>>();

            var d = new Dequeue<object>(poller, processor);
            var result = await d.Run();

            Assert.IsFalse(result);

            poller.Received().Poll();
        }

        [Test]
        public void MinimumPeriodInSeconds()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();

            var d = new Dequeue<object>(poller, processor);
            Assert.AreEqual(BaseTimes.MinimumStorageTiming, d.MinimumPeriodInSeconds);
        }

        [Test]
        public void MaximumPeriodInSeconds()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();

            var d = new Dequeue<object>(poller, processor);
            Assert.AreEqual(BaseTimes.MaximumStorageTiming, d.MaximumPeriodInSeconds);
        }
    }
}
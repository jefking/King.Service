namespace King.Service.Tests
{
    using System;
    using System.Threading.Tasks;
    using global::Azure.Data.Wrappers;
    using King.Service.Data;
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;

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
        public void ConstructorPollerNull()
        {
            var processor = Substitute.For<IProcessor<object>>();
            Assert.That(() => new Dequeue<object>(null, processor), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorProcessorNull()
        {
            var poller = Substitute.For<IPoller<object>>();
            Assert.That(() => new Dequeue<object>(poller, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorMinMaxZero()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();

            var random = new Random();
            new Dequeue<object>(poller, processor, random.Next(0));
        }

        [Test]
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

            await poller.Received().Poll();
        }

        [Test]
        public void MinimumPeriodInSeconds()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();

            var d = new Dequeue<object>(poller, processor);
            Assert.AreEqual(BaseTimes.DefaultMinimumTiming, d.MinimumPeriodInSeconds);
        }

        [Test]
        public void MaximumPeriodInSeconds()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();

            var d = new Dequeue<object>(poller, processor);
            Assert.AreEqual(BaseTimes.DefaultMaximumTiming, d.MaximumPeriodInSeconds);
        }
    }
}
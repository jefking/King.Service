namespace King.Service.Tests.Data
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class DequeueBatchDynamicTests
    {
        [Test]
        public void Constructor()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            var tracker = Substitute.For<ITimingTracker>();
            new DequeueBatchDynamic<object>(poller, processor, tracker);
        }

        [Test]
        public void ConstructorTrackerNull()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            Assert.That(() => new DequeueBatchDynamic<object>(poller, processor, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void IsDequeueBatch()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            var tracker = Substitute.For<ITimingTracker>();
            Assert.IsNotNull(new DequeueBatchDynamic<object>(poller, processor, tracker) as DequeueBatch<object>);
        }

        [Test]
        public void DefaultBatch()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            var tracker = Substitute.For<ITimingTracker>();
            var dbd = new DequeueBatchDynamic<object>(poller, processor, tracker);
            Assert.AreEqual(DequeueBatch<object>.MinimumBatchSize, dbd.BatchCount);
        }

        [Test]
        public async Task Run()
        {
            var data = new object();

            var message = Substitute.For<IQueued<object>>();
            message.Data().Returns(Task.FromResult(data));
            message.Complete();

            var msgs = new List<IQueued<object>>();
            msgs.Add(message);

            var poller = Substitute.For<IPoller<object>>();
            poller.PollMany(1).Returns(Task.FromResult<IEnumerable<IQueued<object>>>(msgs));

            var processor = Substitute.For<IProcessor<object>>();
            processor.Process(data).Returns(Task.FromResult(true));

            var tracker = Substitute.For<ITimingTracker>();
            tracker.Calculate(Arg.Any<TimeSpan>(), 1).Returns<byte>(6);

            var d = new DequeueBatchDynamic<object>(poller, processor, tracker);

            var result = await d.Run();
            Assert.IsTrue(result);

            tracker.Received().Calculate(Arg.Any<TimeSpan>(), 1);
            message.Received().Data();
            message.Received().Complete();
            poller.Received().PollMany(1);
            processor.Received().Process(data);
        }

        [Test]
        public async Task RunPollNull()
        {
            var poller = Substitute.For<IPoller<object>>();
            poller.PollMany(1).Returns(Task.FromResult<IEnumerable<IQueued<object>>>(null));

            var processor = Substitute.For<IProcessor<object>>();
            var tracker = Substitute.For<ITimingTracker>();

            var d = new DequeueBatchDynamic<object>(poller, processor, tracker);
            var result = await d.Run();

            Assert.IsFalse(result);

            poller.Received().PollMany(1);
        }

        [Test]
        public void RunCompleteUp()
        {
            var random = new Random();
            var count = random.Next(5, int.MaxValue);
            var duration = TimeSpan.FromMilliseconds(random.Next());

            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            var tracker = Substitute.For<ITimingTracker>();
            tracker.Calculate(duration, 1).Returns((byte)2);

            var d = new DequeueBatchDynamic<object>(poller, processor, tracker);
            d.RunCompleted(count, duration);

            Assert.AreEqual(2, d.BatchCount);

            tracker.Received().Calculate(duration, 1);
        }

        [Test]
        public void RunCompleteDown()
        {
            var random = new Random();
            var count = random.Next(5, int.MaxValue);
            var duration = TimeSpan.FromMilliseconds(random.Next());

            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            var tracker = Substitute.For<ITimingTracker>();
            tracker.Calculate(duration, 1).Returns((byte)2);
            tracker.Calculate(duration, 2).Returns((byte)1);

            var d = new DequeueBatchDynamic<object>(poller, processor, tracker);
            d.RunCompleted(1, duration);
            d.RunCompleted(1, duration);

            Assert.AreEqual(1, d.BatchCount);

            tracker.Received().Calculate(duration, 1);
            tracker.Received().Calculate(duration, 2);
        }

        [Test]
        [Category("Integration")]
        public void RunCompleteUpIntegration()
        {
            var random = new Random();
            var duration = TimeSpan.FromMilliseconds(10);

            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();

            var d = new DequeueBatchDynamic<object>(poller, processor, new TimingTracker(TimeSpan.FromMinutes(1), 50));
            d.RunCompleted(1, duration);
            d.RunCompleted(2, duration);
            d.RunCompleted(1, duration);
            d.RunCompleted(3, duration);
            d.RunCompleted(1, duration);

            Assert.AreEqual(4, d.BatchCount);
        }

        [Test]
        [Category("Integration")]
        public void RunCompleteDownIntegration()
        {
            var random = new Random();
            var duration = TimeSpan.FromMilliseconds(10);

            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();

            var d = new DequeueBatchDynamic<object>(poller, processor, new TimingTracker(TimeSpan.FromMinutes(1), 50));
            d.RunCompleted(1, duration);
            d.RunCompleted(2, duration);
            d.RunCompleted(3, duration);
            d.RunCompleted(1, TimeSpan.FromMinutes(1));
            d.RunCompleted(1, TimeSpan.FromMinutes(1));
            d.RunCompleted(1, TimeSpan.FromMinutes(1));

            Assert.AreEqual(1, d.BatchCount);
        }
    }
}
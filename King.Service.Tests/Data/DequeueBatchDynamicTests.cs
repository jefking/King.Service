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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTrackerNull()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            new DequeueBatchDynamic<object>(poller, processor, null);
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
    }
}
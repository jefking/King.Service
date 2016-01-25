namespace King.Service.Tests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using King.Azure.Data;
    using King.Service.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DequeueBatchTests
    {
        [Test]
        public void Constructor()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            new DequeueBatch<object>(poller, processor);
        }

        [Test]
        public void BatchCount()
        {
            var random = new Random();
            var count = (byte)random.Next(byte.MinValue, byte.MaxValue);
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            var d = new DequeueBatch<object>(poller, processor, count);
            Assert.AreEqual(count, d.BatchCount);
        }

        [Test]
        public void BatchCountMin()
        {
            var random = new Random();
            var count = byte.MinValue;
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            var d = new DequeueBatch<object>(poller, processor, count);
            Assert.AreEqual(1, d.BatchCount);
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
            poller.PollMany(5).Returns(Task.FromResult<IEnumerable<IQueued<object>>>(msgs));

            var processor = Substitute.For<IProcessor<object>>();
            processor.Process(data).Returns(Task.FromResult(true));

            var d = new DequeueBatch<object>(poller, processor);

            var result = await d.Run();
            Assert.IsTrue(result);

            message.Received().Data();
            message.Received().Complete();
            poller.Received().PollMany(5);
            processor.Received().Process(data);
        }

        [Test]
        public async Task RunPollNull()
        {
            var poller = Substitute.For<IPoller<object>>();
            poller.PollMany(5).Returns(Task.FromResult<IEnumerable<IQueued<object>>>(null));

            var processor = Substitute.For<IProcessor<object>>();

            var d = new DequeueBatch<object>(poller, processor);

            var result = await d.Run();
            Assert.IsFalse(result);

            poller.Received().PollMany(5);
        }

        [Test]
        public async Task RunPollThrows()
        {
            var poller = Substitute.For<IPoller<object>>();
            poller.PollMany(5).Returns(x => { throw new ApplicationException(); });

            var processor = Substitute.For<IProcessor<object>>();

            var d = new DequeueBatch<object>(poller, processor);
            Assert.That(async () => await d.Run(), Throws.TypeOf<ApplicationException>());
        }
    }
}
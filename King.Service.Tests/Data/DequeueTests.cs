﻿namespace King.Service.Tests.Unit.Data
{
    using global::Azure.Data.Wrappers;
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
        public void IsIBackoffRuns()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            Assert.IsNotNull(new Dequeue<object>(poller, processor) as IDynamicRuns);
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
            var random = new Random();
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            new Dequeue<object>(poller, processor, random.Next(0));
        }

        [Test]
        public void ConstructorMinMaxSwitched()
        {
            var random = new Random();
            var value = random.Next(0, 1024);
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            new Dequeue<object>(poller, processor, value + 1, value - 1);
        }

        [Test]
        public void MinimumPeriodInSeconds()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            var d = new Dequeue<object>(poller, processor);
            Assert.AreEqual(10, d.MinimumPeriodInSeconds);
        }

        [Test]
        public void MaximumPeriodInSeconds()
        {
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            var d = new Dequeue<object>(poller, processor);
            Assert.AreEqual(BaseTimes.DefaultMaximumTiming, d.MaximumPeriodInSeconds);
        }

        [Test]
        public async Task Run()
        {
            var data = new object();

            var message = Substitute.For<IQueued<object>>();
            message.Data().Returns(Task.FromResult(data));
            await message.Complete();

            var poller = Substitute.For<IPoller<object>>();
            poller.Poll().Returns(Task.FromResult(message));

            var processor = Substitute.For<IProcessor<object>>();
            processor.Process(data).Returns(Task.FromResult(true));

            var d = new Dequeue<object>(poller, processor);

            var result = await d.Run();
            Assert.IsTrue(result);

            await message.Received().Data();
            await message.Received().Complete();
            await poller.Received().Poll();
            await processor.Received().Process(data);
        }

        [Test]
        public async Task RunPollNull()
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
        public void RunPollThrows()
        {
            var poller = Substitute.For<IPoller<object>>();
            poller.Poll().ReturnsForAnyArgs<object>(x => { throw new ApplicationException(); });

            var processor = Substitute.For<IProcessor<object>>();

            var d = new Dequeue<object>(poller, processor);

            Assert.That(async () => await d.Run(), Throws.TypeOf<ApplicationException>());
        }

        [Test]
        public async Task RunDataNull()
        {
            var message = Substitute.For<IQueued<object>>();
            message.Data().Returns(Task.FromResult<object>(null));

            var poller = Substitute.For<IPoller<object>>();
            poller.Poll().Returns(Task.FromResult(message));

            var processor = Substitute.For<IProcessor<object>>();

            var d = new Dequeue<object>(poller, processor);

            var result = await d.Run();
            Assert.IsTrue(result);

            await poller.Received().Poll();
        }

        [Test]
        public async Task RunDataThrows()
        {
            var message = Substitute.For<IQueued<object>>();
            message.Data().Returns(x => { throw new ApplicationException(); });
            await message.Complete();

            var poller = Substitute.For<IPoller<object>>();
            poller.Poll().Returns(Task.FromResult(message));

            var processor = Substitute.For<IProcessor<object>>();

            var d = new Dequeue<object>(poller, processor);

            Assert.That(async () => await d.Run(), Throws.TypeOf<ApplicationException>());
        }

        [Test]
        public async Task RunProcessFalse()
        {
            var data = new object();
            var message = Substitute.For<IQueued<object>>();
            message.Data().Returns(Task.FromResult<object>(data));

            var poller = Substitute.For<IPoller<object>>();
            poller.Poll().Returns(Task.FromResult(message));

            var processor = Substitute.For<IProcessor<object>>();
            processor.Process(data).Returns(Task.FromResult(false));

            var d = new Dequeue<object>(poller, processor);

            var result = await d.Run();
            Assert.IsTrue(result);

            await message.Received().Data();
            await message.Received().Abandon();
            await poller.Received().Poll();
            await processor.Received().Process(data);
        }

        [Test]
        public void RunProcessThrows()
        {
            var data = new object();
            var message = Substitute.For<IQueued<object>>();
            message.Data().Returns(Task.FromResult<object>(data));

            var poller = Substitute.For<IPoller<object>>();
            poller.Poll().Returns(Task.FromResult(message));

            var processor = Substitute.For<IProcessor<object>>();
            processor.Process(data).ReturnsForAnyArgs<object>(x => { throw new ApplicationException(); });

            var d = new Dequeue<object>(poller, processor);

            Assert.That(async () => await d.Run(), Throws.TypeOf<ApplicationException>());
        }
    }
}
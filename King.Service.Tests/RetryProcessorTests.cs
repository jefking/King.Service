namespace King.Service.Tests.Unit
{
    using King.Service;
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Tests;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class RetryProcessorTests
    {
        [Test]
        public void Constructor()
        {
            new RetryProcessor<int>(new ProcHelper());
        }

        [Test]
        public void ConstructorProcessorNull()
        {
            Assert.That(() => new RetryProcessor<int>(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorMaxZero()
        {
            Assert.That(() => new RetryProcessor<int>(new ProcHelper(), 0), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsIProcessor()
        {
            Assert.IsNotNull(new RetryProcessor<int>(new ProcHelper()) as IProcessor<int>);
        }

        [Test]
        public async Task Process()
        {
            var processor = Substitute.For<IProcessor<int>>();
            processor.Process(1).Returns(true);

            var rp = new RetryProcessor<int>(processor);
            var result = await rp.Process(1);

            Assert.IsTrue(result);
            await processor.Received().Process(1);
        }

        [Test]
        public async Task ProcessSingleFailure()
        {
            var processor = Substitute.For<IProcessor<int>>();
            processor.Process(1).Returns(false, true);

            var rp = new RetryProcessor<int>(processor);
            var result = await rp.Process(1);

            Assert.IsTrue(result);
            await processor.Received(2).Process(1);
        }

        [Test]
        public async Task ProcessFullFailure()
        {
            var processor = Substitute.For<IProcessor<int>>();
            processor.Process(1).Returns(false);

            var rp = new RetryProcessor<int>(processor);
            var result = await rp.Process(1);

            Assert.IsFalse(result);
            await processor.Received(3).Process(1);
        }
    }
}
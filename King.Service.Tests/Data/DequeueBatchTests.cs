namespace King.Service.Tests.Data
{
    using King.Azure.Data;
    using King.Service.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

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
            var count = random.Next(1, 1000);
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            var d = new DequeueBatch<object>(poller, processor, count);
            Assert.AreEqual(count, d.BatchCount);
        }

        [Test]
        public void BatchCountNegative()
        {
            var random = new Random();
            var count = random.Next(1, 1000) * -1;
            var poller = Substitute.For<IPoller<object>>();
            var processor = Substitute.For<IProcessor<object>>();
            var d = new DequeueBatch<object>(poller, processor, count);
            Assert.AreEqual(5, d.BatchCount);
        }
    }
}
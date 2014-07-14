namespace King.Azure.BackgroundWorker.Tests.Data
{
    using King.Azure.BackgroundWorker.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;

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
    }
}
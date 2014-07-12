namespace King.Azure.BackgroundWorker.Tests
{
    using King.Azure.BackgroundWorker.Data;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class DequeueTests
    {
        [Test]
        public void Constructor()
        {
            new Dequeue();
        }

        [Test]
        public void IsIBackoffRuns()
        {
            Assert.IsNotNull(new Dequeue() as IBackoffRuns);
        }

        [Test]
        public void Run()
        {
            var d = new Dequeue();
            Assert.IsFalse(d.Run());
        }

        [Test]
        public void MinimumPeriodInSeconds()
        {
            var d = new Dequeue();
            Assert.AreEqual(1, d.MinimumPeriodInSeconds);
        }

        [Test]
        public void MaximumPeriodInSeconds()
        {
            var d = new Dequeue();
            Assert.AreEqual(600, d.MaximumPeriodInSeconds);
        }
    }
}
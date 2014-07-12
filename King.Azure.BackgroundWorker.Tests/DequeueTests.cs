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
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinMaxZero()
        {
            var random = new Random();
            new Dequeue(random.Next(0));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinMaxSwitched()
        {
            var random = new Random();
            var value = random.Next(0, 1024);
            new Dequeue(value + 1, value - 1);
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
            Assert.AreEqual(15, d.MinimumPeriodInSeconds);
        }

        [Test]
        public void MaximumPeriodInSeconds()
        {
            var d = new Dequeue();
            Assert.AreEqual(300, d.MaximumPeriodInSeconds);
        }
    }
}
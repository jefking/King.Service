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
        public void IsBackoffTask()
        {
            Assert.IsNotNull(new Dequeue() as BackoffTask);
        }

        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void Run()
        {
            var work = false;
            var d = new Dequeue();
            d.Run(out work);
        }
    }
}
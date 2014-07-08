namespace King.Azure.BackgroundWorker.Tests
{

using NUnit.Framework;
    using System;

    [TestFixture]
    public class TimingTests
    {
        [Test]
        public void Constructor()
        {
            new Timing();
        }
        
        [Test]
        public void IsITiming()
        {
            Assert.IsNotNull(new Timing() as ITiming);
        }

        [Test]
        public void Attempt()
        {
            var random = new Random();
            var min = random.Next();
            var time = new Timing();
            var ex = time.Exponential(min, 60, 0);
            Assert.AreEqual(min, ex);
        }

        [Test]
        public void AttemptMax()
        {
            var random = new Random();
            var max = random.Next(1, 60);
            var time = new Timing();
            var ex = time.Exponential(0, max, (ulong)random.Next(61, 500));
            Assert.AreEqual(max, ex);
        }

        [Test]
        public void FirstAttempt()
        {
            var time = new Timing();
            var ex = time.Exponential(0, 60, 1);
            Assert.AreEqual(2, ex);
        }
        
        [Test]
        public void SecondAttempt()
        {
            var time = new Timing();
            var ex = time.Exponential(0, 60, 2);
            Assert.AreEqual(4, ex);
        }

        [Test]
        public void ThirdAttempt()
        {
            var time = new Timing();
            var ex = time.Exponential(0, 60, 3);
            Assert.AreEqual(8, ex);
        }

        [Test]
        public void FourthAttempt()
        {
            var time = new Timing();
            var ex = time.Exponential(0, 60, 4);
            Assert.AreEqual(16, ex);
        }
    }
}
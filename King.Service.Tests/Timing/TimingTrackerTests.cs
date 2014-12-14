namespace King.Service.Tests.Timing
{
    using King.Service.Data;
    using King.Service.Timing;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class TimingTrackerTests
    {
        [Test]
        public void Constructor()
        {
            new TimingTracker(TimeSpan.FromSeconds(10));
        }

        [Test]
        public void IsITimingTracker()
        {
            Assert.IsNotNull(new TimingTracker(TimeSpan.FromSeconds(10)) as ITimingTracker);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMaxZero()
        {
            new TimingTracker(TimeSpan.Zero);
        }

        [Test]
        public void Maximum()
        {
            var t = new TimingTracker(TimeSpan.FromSeconds(10));
            var result = t.Calculate(TimeSpan.FromSeconds(1), byte.MaxValue);
            Assert.AreEqual(byte.MaxValue, result);
        }

        [Test]
        public void MaximumSpecified()
        {
            var t = new TimingTracker(TimeSpan.FromSeconds(10), 22);
            var result = t.Calculate(TimeSpan.FromSeconds(1), byte.MaxValue);
            Assert.AreEqual(22, result);
        }

        [Test]
        public void Minimum()
        {
            var t = new TimingTracker(TimeSpan.FromSeconds(10));
            var result = t.Calculate(TimeSpan.FromSeconds(11), byte.MinValue);
            Assert.AreEqual(DequeueBatch<object>.MinimumBatchSize, result);
        }

        [Test]
        public void Up()
        {
            var t = new TimingTracker(TimeSpan.FromSeconds(10));
            var result = t.Calculate(TimeSpan.FromSeconds(1), 1);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void Down()
        {
            var t = new TimingTracker(TimeSpan.FromSeconds(10));
            var result = t.Calculate(TimeSpan.FromSeconds(11), 5);
            Assert.AreEqual(4, result);
        }
    }
}
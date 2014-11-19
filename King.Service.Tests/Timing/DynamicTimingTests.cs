namespace King.Service.Tests.Timing
{
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class DynamicTimingTests
    {
        [Test]
        public void Constructor()
        {
            var random = new Random();
            new BackoffTiming(random.Next(1, 100), random.Next(100, 1000));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTimingNull()
        {
            new AdaptiveTiming(null);
        }

        [Test]
        public void IsIDynamicTiming()
        {
            var random = new Random();
            Assert.IsNotNull(new BackoffTiming(random.Next(1, 100), random.Next(100, 1000)) as IDynamicTiming);
        }

        [Test]
        public void Timing()
        {
            var timing = Substitute.For<ICalculateTiming>();

            var t = new AdaptiveTiming(timing);

            Assert.AreEqual(timing, t.Timing);
        }
    }
}
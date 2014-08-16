namespace King.Azure.BackgroundWorker.Tests
{
    using King.Service;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class AdaptiveTimingTests
    {
        [Test]
        public void Constructor()
        {
            new AdaptiveTiming();
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
            Assert.IsNotNull(new AdaptiveTiming() as IDynamicTiming);
        }

        [Test]
        public void Get()
        {
            var random = new Random();
            var min = random.Next();
            var max = random.Next();
            var expected = random.NextDouble();
            var timing = Substitute.For<ITiming>();
            timing.Exponential(0, max, min).Returns(expected);

            var t = new AdaptiveTiming(timing);
            var value = t.Get(true, max, min);

            Assert.AreEqual(expected, value);

            timing.Received().Exponential(0, max, min);
        }
    }
}
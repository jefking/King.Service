namespace King.Service.Tests.Timing
{
    using King.Service;
    using King.Service.Timing;
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
            var timing = Substitute.For<ICalculateTiming>();
            timing.Exponential(0, max, min).Returns(expected);

            var t = new AdaptiveTiming(timing);
            var value = t.Get(true, max, min);

            Assert.AreEqual(expected, value);

            timing.Received().Exponential(0, max, min);
        }

        [Test]
        public void GetNoWork()
        {
            var random = new Random();
            var min = random.Next();
            var max = random.Next();
            var expected = random.NextDouble();
            var timing = Substitute.For<ICalculateTiming>();
            timing.Exponential(1, max, min).Returns(expected);

            var t = new AdaptiveTiming(timing);
            var value = t.Get(false, max, min);

            Assert.AreEqual(expected, value);

            timing.Received().Exponential(1, max, min);
        }

        [Test]
        public void GetWorkMultiple()
        {
            var random = new Random();
            var min = random.Next();
            var max = random.Next();
            var expected = random.NextDouble();
            var timing = Substitute.For<ICalculateTiming>();
            timing.Exponential(0, max, min).Returns(expected);

            var t = new AdaptiveTiming(timing);
            t.Get(true, max, min);
            t.Get(true, max, min);
            t.Get(true, max, min);
            t.Get(true, max, min);
            t.Get(true, max, min);
            var value = t.Get(true, max, min);

            Assert.AreEqual(expected, value);

            timing.Received(6).Exponential(0, max, min);
        }

        [Test]
        public void GetNoWorkMultiple()
        {
            var random = new Random();
            var min = random.Next();
            var max = random.Next();
            var expected = random.NextDouble();
            var timing = Substitute.For<ICalculateTiming>();
            timing.Exponential(6, max, min).Returns(expected);

            var t = new AdaptiveTiming(timing);
            t.Get(false, max, min);
            t.Get(false, max, min);
            t.Get(false, max, min);
            t.Get(false, max, min);
            t.Get(false, max, min);
            var value = t.Get(false, max, min);

            Assert.AreEqual(expected, value);

            timing.Received().Exponential(1, max, min);
            timing.Received().Exponential(2, max, min);
            timing.Received().Exponential(3, max, min);
            timing.Received().Exponential(4, max, min);
            timing.Received().Exponential(5, max, min);
            timing.Received().Exponential(6, max, min);
        }

        [Test]
        public void GetWorkNoWork()
        {
            var random = new Random();
            var min = random.Next();
            var max = random.Next();
            var expected = random.NextDouble();
            var timing = Substitute.For<ICalculateTiming>();
            timing.Exponential(0, max, min).Returns(expected);
            timing.Exponential(1, max, min).Returns(expected);
            timing.Exponential(2, max, min).Returns(expected);

            var t = new AdaptiveTiming(timing);
            t.Get(true, max, min);
            t.Get(false, max, min);
            t.Get(true, max, min);
            t.Get(false, max, min);
            t.Get(false, max, min);
            var value = t.Get(true, max, min);

            Assert.AreEqual(expected, value);

            timing.Received(2).Exponential(0, max, min);
            timing.Received(3).Exponential(1, max, min);
            timing.Received().Exponential(2, max, min);
        }
    }
}
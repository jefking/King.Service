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
        public void IsDynamicTiming()
        {
            var random = new Random();
            Assert.IsNotNull(new AdaptiveTiming(random.Next(1, 100), random.Next(100, 1000)) as DynamicTiming);
        }

        [Test]
        public void Timing()
        {
            var timing = Substitute.For<ICalculateTiming>();

            var t = new AdaptiveTiming(timing);

            Assert.AreEqual(timing, t.Timing);
        }

        [Test]
        public void Get()
        {
            var random = new Random();
            var expected = random.NextDouble();
            var timing = Substitute.For<ICalculateTiming>();
            timing.Get(0).Returns(expected);

            var t = new AdaptiveTiming(timing);
            var value = t.Get(true);

            Assert.AreEqual(expected, value);

            timing.Received().Get(0);
        }

        [Test]
        public void GetNoWork()
        {
            var random = new Random();
            var expected = random.NextDouble();
            var timing = Substitute.For<ICalculateTiming>();
            timing.Get(0).Returns(expected);

            var t = new AdaptiveTiming(timing);
            var value = t.Get(false);

            Assert.AreEqual(expected, value);

            timing.Received().Get(0);
        }

        [Test]
        public void GetWorkMultiple()
        {
            var random = new Random();
            var expected = random.NextDouble();
            var timing = Substitute.For<ICalculateTiming>();
            timing.Get(0).Returns(expected);

            var t = new AdaptiveTiming(timing);
            t.Get(true);
            t.Get(true);
            t.Get(true);
            t.Get(true);
            t.Get(true);
            var value = t.Get(true);

            Assert.AreEqual(expected, value);

            timing.Received(6).Get(0);
        }

        [Test]
        public void GetNoWorkMultiple()
        {
            var random = new Random();
            var expected = random.NextDouble();
            var timing = Substitute.For<ICalculateTiming>();
            timing.MaximumPeriodInSeconds.Returns(100000);
            timing.Get(6).Returns(expected);

            var t = new AdaptiveTiming(timing);
            t.Get(false);
            t.Get(false);
            t.Get(false);
            t.Get(false);
            t.Get(false);
            var value = t.Get(false);

            Assert.AreEqual(expected, value);

            timing.Received().Get(1);
            timing.Received().Get(2);
            timing.Received().Get(3);
            timing.Received().Get(4);
            timing.Received().Get(5);
            timing.Received().Get(6);
            var r = timing.Received().MaximumPeriodInSeconds;
        }

        [Test]
        public void GetWorkNoWork()
        {
            var random = new Random();
            var expected = random.NextDouble();
            var timing = Substitute.For<ICalculateTiming>();
            timing.MaximumPeriodInSeconds.Returns(100000);
            timing.Get(0).Returns(expected);
            timing.Get(1).Returns(expected);
            timing.Get(2).Returns(expected);

            var t = new AdaptiveTiming(timing);
            t.Get(true);
            t.Get(false);
            t.Get(true);
            t.Get(false);
            t.Get(false);
            var value = t.Get(true);

            Assert.AreEqual(expected, value);

            timing.Received(4).Get(0);
            timing.Received(4).Get(1);
            timing.Received().Get(2);
            var r = timing.Received().MaximumPeriodInSeconds;
        }

        [Test]
        public void GetMaxThenMin()
        {
            var min = 1;
            var max = 10;
            var t = new AdaptiveTiming(min, max);
            while (max > t.Get(false))
            { }

            Assert.AreEqual(max, t.Get(false));
            Assert.AreEqual(max, t.Get(false));
            
            var result = t.Get(true);
            Assert.IsTrue(result < max);
        }
    }
}
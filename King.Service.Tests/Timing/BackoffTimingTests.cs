namespace King.Service.Tests.Unit.Timing
{
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BackoffTimingTests
    {
        [Test]
        public void IsDynamicTiming()
        {
            var random = new Random();
            Assert.IsNotNull(new BackoffTiming(random.Next(1, 100), random.Next(100, 1000)) as DynamicTiming);
        }

        [Test]
        public void Get()
        {
            var random = new Random();
            var expected = random.Next();
            var timing = Substitute.For<ICalculateTiming>();
            timing.FrequencyInSeconds.Returns(new Range<int>(expected, expected* 2));

            var t = new BackoffTiming(timing);
            var value = t.Get(true);

            Assert.AreEqual(expected, value);

            var r = timing.Received().FrequencyInSeconds;
        }

        [Test]
        public void GetNoWork()
        {
            var random = new Random();
            var expected = random.NextDouble();
            var timing = Substitute.For<ICalculateTiming>();
            timing.Get(1).Returns(expected);

            var t = new BackoffTiming(timing);
            var value = t.Get(false);

            Assert.AreEqual(expected, value);

            timing.Received().Get(1);
        }

        [Test]
        public void GetWorkMultiple()
        {
            var random = new Random();
            var expected = random.Next();
            var timing = Substitute.For<ICalculateTiming>();
            timing.FrequencyInSeconds.Returns(new Range<int>(expected, expected * 2));

            var t = new BackoffTiming(timing);
            t.Get(true);
            t.Get(true);
            t.Get(true);
            t.Get(true);
            t.Get(true);
            var value = t.Get(true);

            Assert.AreEqual(expected, value);

            var r = timing.Received(6).FrequencyInSeconds;
        }

        [Test]
        public void GetNoWorkMultiple()
        {
            var random = new Random();
            var expected = random.NextDouble();
            var timing = Substitute.For<ICalculateTiming>();
            timing.Get(1).Returns(random.NextDouble());
            timing.Get(2).Returns(random.NextDouble());
            timing.Get(3).Returns(random.NextDouble());
            timing.Get(4).Returns(random.NextDouble());
            timing.Get(5).Returns(random.NextDouble());
            timing.Get(6).Returns(expected);

            var t = new BackoffTiming(timing);
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
        }

        [Test]
        public void GetMaxThenMin()
        {
            var min = 1;
            var max = 10;
            var t = new BackoffTiming(min, max);
            while (max > t.Get(false))
            { }

            Assert.AreEqual(max, t.Get(false));
            Assert.AreEqual(max, t.Get(false));

            var result = t.Get(true);
            Assert.IsTrue(result < max);
        }
    }
}

namespace King.Service.Tests.Timing
{
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BackoffTimingTests
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
            new BackoffTiming(null);
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

            var t = new BackoffTiming(timing);

            Assert.AreEqual(timing, t.Timing);
        }

        [Test]
        public void Get()
        {
            var random = new Random();
            var expected = random.NextDouble();
            var timing = Substitute.For<ICalculateTiming>();
            timing.Get(0).Returns(expected);

            var t = new BackoffTiming(timing);
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
            var expected = random.NextDouble();
            var timing = Substitute.For<ICalculateTiming>();
            timing.Get(0).Returns(expected);

            var t = new BackoffTiming(timing);
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
    }
}

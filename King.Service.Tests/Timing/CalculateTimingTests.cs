namespace King.Service.Tests.Timing
{
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class TimingTests
    {
        [Test]
        public void Constructor()
        {
            var random = new Random();
            new LinearTiming(random.Next(1, 100), random.Next(100, 1000));
        }

        [Test]
        public void IsITiming()
        {
            var random = new Random();
            Assert.IsNotNull(new ExponentialTiming(random.Next(1, 100), random.Next(100, 1000)) as ICalculateTiming);
        }

        [Test]
        public void ConstructorMinMaxZero()
        {
            var random = new Random();
            new LinearTiming(0, 0);
        }

        [Test]
        public void ConstructorMinMaxSwitched()
        {
            var random = new Random();
            new ExponentialTiming(random.Next(100, 100), random.Next(1, 10));
        }

        [Test]
        public void MinimumPeriodInSeconds()
        {
            var random = new Random();
            var min = random.Next(1, 99);
            var ct = new LinearTiming(min, random.Next(101, 1000));

            Assert.AreEqual(min, ct.MinimumPeriodInSeconds);
        }
    }
}
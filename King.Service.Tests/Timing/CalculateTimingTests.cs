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
            new CalculateTiming(random.Next(1, 100), random.Next(100, 1000));
        }

        [Test]
        public void IsITiming()
        {
            var random = new Random();
            Assert.IsNotNull(new CalculateTiming(random.Next(1, 100), random.Next(100, 1000)) as ICalculateTiming);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinMaxZero()
        {
            var random = new Random();
            new CalculateTiming(0, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinMaxSwitched()
        {
            var random = new Random();
            new CalculateTiming(random.Next(100, 100), random.Next(1, 10));
        }

        [Test]
        public void MinimumPeriodInSeconds()
        {
            var random = new Random();
            var min = random.Next(1, 99);
            var ct = new CalculateTiming(min, random.Next(101, 1000));

            Assert.AreEqual(min, ct.MinimumPeriodInSeconds);
        }

        [Test]
        public void AttemptMinimum()
        {
            var random = new Random();
            var min = random.Next(1, 10);
            var time = new CalculateTiming(min, random.Next(100, 1000));
            var ex = time.Exponential(0);
            Assert.AreEqual(min, ex);
        }

        [Test]
        public void AttemptMaximmum()
        {
            var random = new Random();
            var max = random.Next(100, 1000);
            var time = new CalculateTiming(random.Next(1, 10), max);
            var ex = time.Exponential((ulong)random.Next(61, 500));
            Assert.AreEqual(max, ex);
        }

        [Test]
        public void AttemptsSmall()
        {
            var random = new Random();
            var min = random.Next(1, 30);
            var max = random.Next(60, 120);

            var time = new CalculateTiming(min, max);
            for (ulong i = 1; i < 10; i++)
            {
                var calc = time.Exponential(i);

                var expected = ((Math.Pow(2, i) * .1d) * min) + min;
                if (expected > max)
                {
                    break;// Not testing max
                }
                else
                {
                    Assert.AreEqual(expected, calc);
                }
            }
        }

        [Test]
        public void AttemptsLarge()
        {
            var random = new Random();
            var min = random.Next(3600, 4000);
            var max = random.Next(28800, 86400);

            var time = new CalculateTiming(min, max);
            for (ulong i = 1; i < 10; i++)
            {
                var calc = time.Exponential(i);

                var expected = ((Math.Pow(2, i) * .1d) * min) + min;
                if (expected > max)
                {
                    break;// Not testing max
                }
                else
                {
                    Assert.AreEqual(expected, calc);
                }
            }
        }
    }
}
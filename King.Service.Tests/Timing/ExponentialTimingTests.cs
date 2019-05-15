namespace King.Service.Tests.Unit.Timing
{
    using King.Service.Timing;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ExponentialTimingTests
    {
        [Test]
        public void IsCalculateTiming()
        {
            var random = new Random();
            Assert.IsNotNull(new ExponentialTiming(random.Next(1, 100), random.Next(100, 1000)) as CalculateTiming);
        }

        [Test]
        public void AttemptMinimum()
        {
            var random = new Random();
            var min = random.Next(1, 10);
            var time = new ExponentialTiming(min, random.Next(100, 1000));
            var ex = time.Get(0);
            Assert.AreEqual(min, ex);
        }

        [Test]
        public void AttemptMaximmum()
        {
            var random = new Random();
            var max = random.Next(100, 1000);
            var time = new ExponentialTiming(random.Next(1, 10), max);
            var ex = time.Get((ulong)random.Next(61, 500));
            Assert.AreEqual(max, ex);
        }

        [Test]
        public void AttemptsSmall()
        {
            var random = new Random();
            var min = random.Next(1, 30);
            var max = random.Next(60, 120);

            var time = new ExponentialTiming(min, max);
            for (ulong i = 1; i < 10; i++)
            {
                var calc = time.Get(i);

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

            var time = new ExponentialTiming(min, max);
            for (ulong i = 1; i < 10; i++)
            {
                var calc = time.Get(i);

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
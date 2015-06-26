namespace King.Service.Tests.Timing
{
    using King.Service.Timing;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class StrategiesTests
    {
        [Test]
        public void GetExponential()
        {
            var random = new Random();
            var min = random.Next(1, 100);
            var max = random.Next(101, 1000);
            var t = Strategies.Get(Strategy.Exponential, min, max);
            Assert.IsNotNull(t as ExponentialTiming);
            Assert.AreEqual(min, t.FrequencyInSeconds.Minimum);
        }

        [Test]
        public void GetLinear()
        {
            var random = new Random();
            var min = random.Next(1, 100);
            var max = random.Next(101, 1000);
            var t = Strategies.Get(Strategy.Linear, min, max);
            Assert.IsNotNull(t as LinearTiming);
            Assert.AreEqual(min, t.FrequencyInSeconds.Minimum);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetUnknown()
        {
            Strategies.Get((Strategy)22, 100, 100);
        }

        [Test]
        public void AdaptiveLinear()
        {
            var random = new Random();
            var min = random.Next(1, 100);
            var max = random.Next(101, 1000);
            var t = Strategies.Adaptive(Strategy.Linear, min, max);
            Assert.IsNotNull(t as AdaptiveTiming);
            Assert.IsNotNull(t.Timing as LinearTiming);
        }

        [Test]
        public void AdaptiveExponential()
        {
            var random = new Random();
            var min = random.Next(1, 100);
            var max = random.Next(101, 1000);
            var t = Strategies.Adaptive(Strategy.Exponential, min, max);
            Assert.IsNotNull(t as AdaptiveTiming);
            Assert.IsNotNull(t.Timing as ExponentialTiming);
        }

        [Test]
        public void BackoffLinear()
        {
            var random = new Random();
            var min = random.Next(1, 100);
            var max = random.Next(101, 1000);
            var t = Strategies.Backoff(Strategy.Linear, min, max);
            Assert.IsNotNull(t as BackoffTiming);
            Assert.IsNotNull(t.Timing as LinearTiming);
        }

        [Test]
        public void BackoffExponential()
        {
            var random = new Random();
            var min = random.Next(1, 100);
            var max = random.Next(101, 1000);
            var t = Strategies.Backoff(Strategy.Exponential, min, max);
            Assert.IsNotNull(t as BackoffTiming);
            Assert.IsNotNull(t.Timing as ExponentialTiming);
        }
    }
}
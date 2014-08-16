namespace King.Service.Tests
{
    using King.Service;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class AdaptiveTaskTests
    {
        #region Helper
        public class AdaptiveTest : AdaptiveTask
        {
            public AdaptiveTest(int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
                : base(minimumPeriodInSeconds, maximumPeriodInSeconds)
            {
            }
            public bool Work
            {
                get;
                set;
            }
            public override void Run(out bool workWasDone)
            {
                workWasDone = this.Work;
            }
        }
        #endregion

        [Test]
        public void Constructor()
        {
            using (new AdaptiveTest())
            {
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinMaxZero()
        {
            var random = new Random();
            using (new AdaptiveTest(random.Next(0)))
            {
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinMaxSwitched()
        {
            var random = new Random();
            var value = random.Next(0, 1024);
            using (new AdaptiveTest(value + 1, value - 1))
            {
            }
        }
    }
}
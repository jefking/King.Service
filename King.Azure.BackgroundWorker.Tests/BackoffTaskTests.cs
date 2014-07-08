namespace King.Azure.BackgroundWorker.Tests
{
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BackoffTaskTests
    {
        #region Helper
        public class BackoffTest : BackoffTask
        {
            public BackoffTest(int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
                : base(minimumPeriodInSeconds, maximumPeriodInSeconds)
            {
            }
            public BackoffTest(ITiming timing, int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
                : base(timing, minimumPeriodInSeconds, minimumPeriodInSeconds)
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
            new BackoffTest();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTimingNull()
        {
            new BackoffTest(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinMaxZero()
        {
            var random = new Random();
            var time = Substitute.For<ITiming>();
            new BackoffTest(time, random.Next(0));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinMaxSwitched()
        {
            var random = new Random();
            var value = random.Next(0, 1024);
            var time = Substitute.For<ITiming>();
            new BackoffTest(time, value + 1, value - 1);
        }
    }
}
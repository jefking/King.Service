namespace King.Service.Tests
{
    using King.Service;
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
                : base(timing, minimumPeriodInSeconds, maximumPeriodInSeconds)
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
            using (new BackoffTest())
            {
            }
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
            using (new BackoffTest(time, random.Next(0)))
            {
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinMaxSwitched()
        {
            var random = new Random();
            var value = random.Next(0, 1024);
            var time = Substitute.For<ITiming>();
            using (new BackoffTest(time, value + 1, value - 1))
            {
            }
        }

        [Test]
        public void Run()
        {
            var random = new Random();
            var min = random.Next(1, 30);
            var max = random.Next(90, 1024);
            var time = Substitute.For<ITiming>();
            time.Exponential(1, max, min).Returns(4);

            using (var task = new BackoffTest(time, min, max))
            {
                task.Run();
            }

            time.Received().Exponential(1, max, min);
        }

        [Test]
        public void RunWorkDone()
        {
            var random = new Random();
            var min = random.Next(1, 30);
            var max = random.Next(90, 1024);
            var time = Substitute.For<ITiming>();
            time.Exponential(0, max, min).Returns(99);

            using (var task = new BackoffTest(time, min, max))
            {
                task.Work = true;
                task.Run();
            }

            time.Received().Exponential(0, max, min);
        }
    }
}
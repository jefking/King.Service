namespace King.Service.Tests
{
    using King.Service;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class SelfGoverningTaskTests
    {
        #region Helper
        public class SelfGoverningTest : SelfGoverningTask
        {
            public SelfGoverningTest(int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
                : base(minimumPeriodInSeconds, maximumPeriodInSeconds)
            {
            }
            public SelfGoverningTest(ITiming timing, int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
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
            using (new SelfGoverningTest())
            {
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTimingNull()
        {
            new SelfGoverningTest(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinMaxZero()
        {
            var random = new Random();
            var time = Substitute.For<ITiming>();
            using (new SelfGoverningTest(time, random.Next(0)))
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
            using (new SelfGoverningTest(time, value + 1, value - 1))
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

            using (var task = new SelfGoverningTest(time, min, max))
            {
                task.Run();
            }

            time.Received().Exponential(1, max, min);
        }

        [Test]
        public void RunStepDown()
        {
            var random = new Random();
            var min = random.Next(1, 30);
            var max = random.Next(90, 1024);
            var time = Substitute.For<ITiming>();
            time.Exponential(1, max, min).Returns(99);
            time.Exponential(2, max, min).Returns(99);
            time.Exponential(3, max, min).Returns(99);

            using (var task = new SelfGoverningTest(time, min, max))
            {
                task.Run();
                task.Run();
                task.Run();
            }

            time.Exponential(1, max, min).Returns(99);
            time.Exponential(2, max, min).Returns(99);
            time.Exponential(3, max, min).Returns(99);
        }

        [Test]
        public void RunWorkDone()
        {
            var random = new Random();
            var min = random.Next(1, 30);
            var max = random.Next(90, 1024);
            var time = Substitute.For<ITiming>();
            time.Exponential(-1, max, min).Returns(99);

            using (var task = new SelfGoverningTest(time, min, max))
            {
                task.Work = true;
                task.Run();
            }

            time.Received().Exponential(-1, max, min);
        }

        [Test]
        public void RunStepUp()
        {
            var random = new Random();
            var min = random.Next(1, 30);
            var max = random.Next(90, 1024);
            var time = Substitute.For<ITiming>();
            time.Exponential(-1, max, min).Returns(99);
            time.Exponential(-2, max, min).Returns(99);
            time.Exponential(-3, max, min).Returns(99);

            using (var task = new SelfGoverningTest(time, min, max))
            {
                task.Work = true;
                task.Run();
                task.Run();
                task.Run();
            }

            time.Received().Exponential(-1, max, min);
            time.Received().Exponential(-2, max, min);
            time.Received().Exponential(-3, max, min);
        }
    }
}
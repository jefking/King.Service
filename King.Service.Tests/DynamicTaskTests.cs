namespace King.Service.Tests
{
    using King.Service;
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class DynamicTaskTests
    {
        #region Helper
        public class DynamicTest : DynamicTask
        {
            public DynamicTest(int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
                : this(new BackoffTiming(minimumPeriodInSeconds, maximumPeriodInSeconds), minimumPeriodInSeconds, maximumPeriodInSeconds)
            {
            }
            public DynamicTest(IDynamicTiming timing, int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
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
            using (new DynamicTest())
            {
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTimingNull()
        {
            new DynamicTest(null);
        }

        [Test]
        public void Run()
        {
            var random = new Random();
            var time = Substitute.For<IDynamicTiming>();
            time.Get(false).Returns(4);

            using (var task = new DynamicTest(time))
            {
                task.Run();
            }

            time.Received().Get(false);
        }

        [Test]
        public void RunStepDown()
        {
            var random = new Random();
            var time = Substitute.For<IDynamicTiming>();
            time.Get(false).Returns(99);
            time.Get(false).Returns(99);
            time.Get(false).Returns(99);

            using (var task = new DynamicTest(time))
            {
                task.Run();
                task.Run();
                task.Run();
            }

            time.Get(false).Returns(99);
            time.Get(false).Returns(99);
            time.Get(false).Returns(99);
        }

        [Test]
        public void RunWorkDone()
        {
            var random = new Random();
            var min = random.Next(1, 30);
            var max = random.Next(90, 1024);
            var time = Substitute.For<IDynamicTiming>();
            time.Get(true).Returns(99);

            using (var task = new DynamicTest(time, min, max))
            {
                task.Work = true;
                task.Run();
            }

            time.Received().Get(true);
        }

        [Test]
        public void RunStepUp()
        {
            var random = new Random();
            var min = random.Next(1, 30);
            var max = random.Next(90, 1024);
            var time = Substitute.For<IDynamicTiming>();
            time.Get(true).Returns(99);
            
            using (var task = new DynamicTest(time))
            {
                task.Work = true;
                task.Run();
                task.Run();
                task.Run();
            }

            time.Received(3).Get(true);
        }
    }
}
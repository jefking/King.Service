namespace King.Service.Tests.Unit
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
                : this(new BackoffTiming(minimumPeriodInSeconds, maximumPeriodInSeconds))
            {
            }
            public DynamicTest(IDynamicTiming timing)
                : base(timing)
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
        public void ConstructorTimingNull()
        {
            Assert.That(() => new DynamicTest(null), Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void Scale()
        {
            var timing = Substitute.For<ICalculateTiming>();
            timing.FrequencyInSeconds.Returns(new Range<int>(60, 90));
            var time = Substitute.For<IDynamicTiming>();
            time.Timing.Returns(timing);
            time.Get(false).Returns(65);

            using (var task = new DynamicTest(time))
            {
                task.Run();
                Assert.IsTrue(task.Scale);
            }

            var t = time.Received().Timing;
            var mpins = timing.Received().FrequencyInSeconds;
        }

        [Test]
        public void ScaleNope()
        {
            var timing = Substitute.For<ICalculateTiming>();
            timing.FrequencyInSeconds.Returns(new Range<int>(60, 300));
            var time = Substitute.For<IDynamicTiming>();
            time.Timing.Returns(timing);

            using (var task = new DynamicTest(time))
            {
                Assert.IsFalse(task.Scale);
            }

            var t = time.Received().Timing;
            var mpins = timing.Received().FrequencyInSeconds;
        }

        [Test]
        public void Run()
        {
            var random = new Random();
            var time = Substitute.For<IDynamicTiming>();
            var timing = Substitute.For<ICalculateTiming>();
            timing.FrequencyInSeconds.Returns(new Range<int>(1, 2));
            time.Timing.Returns(timing);
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
            var timing = Substitute.For<ICalculateTiming>();
            timing.FrequencyInSeconds.Returns(new Range<int>(1, 2));
            time.Timing.Returns(timing);
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
            var time = Substitute.For<IDynamicTiming>();
            var timing = Substitute.For<ICalculateTiming>();
            timing.FrequencyInSeconds.Returns(new Range<int>(1, 2));
            time.Timing.Returns(timing);
            time.Get(true).Returns(99);

            using (var task = new DynamicTest(time))
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
            var timing = Substitute.For<ICalculateTiming>();
            timing.FrequencyInSeconds.Returns(new Range<int>(1, 2));
            time.Timing.Returns(timing);
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
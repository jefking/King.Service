namespace King.Service.Tests.Data
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Data.Model;
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class StorageQueueAutoScalerTests
    {
        const string ConnectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            new StorageQueueAutoScaler<object>(count, setup);
        }

        [Test]
        public void IsQueueAutoScaler()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            Assert.IsNotNull(new StorageQueueAutoScaler<object>(count, setup) as QueueAutoScaler<IQueueSetup<object>>);
        }

        [Test]
        public void RunsLow()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            setup.Priority.Returns(QueuePriority.Low);
            setup.Name.Returns(Guid.NewGuid().ToString());
            setup.ConnectionString.Returns(ConnectionString);
            setup.Get().Returns(Substitute.For<IProcessor<object>>());

            var s = new StorageQueueAutoScaler<object>(count, setup);
            var runs = s.Runs(setup);

            Assert.IsNotNull(runs);
            Assert.AreEqual(BaseTimes.MinimumStorageTiming, runs.MinimumPeriodInSeconds);
            Assert.AreEqual(BaseTimes.MaximumStorageTiming, runs.MaximumPeriodInSeconds);
        }

        [Test]
        public void RunsMedium()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            setup.Priority.Returns(QueuePriority.Medium);
            setup.Name.Returns(Guid.NewGuid().ToString());
            setup.ConnectionString.Returns(ConnectionString);
            setup.Get().Returns(Substitute.For<IProcessor<object>>());

            var s = new StorageQueueAutoScaler<object>(count, setup);
            var runs = s.Runs(setup);

            Assert.IsNotNull(runs);
            Assert.AreEqual(7, runs.MinimumPeriodInSeconds);
            Assert.AreEqual(90, runs.MaximumPeriodInSeconds);
        }

        [Test]
        public void RunsHigh()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            setup.Priority.Returns(QueuePriority.High);
            setup.Name.Returns(Guid.NewGuid().ToString());
            setup.ConnectionString.Returns(ConnectionString);
            setup.Get().Returns(Substitute.For<IProcessor<object>>());

            var s = new StorageQueueAutoScaler<object>(count, setup);
            var runs = s.Runs(setup);

            Assert.IsNotNull(runs);
            Assert.AreEqual(1, runs.MinimumPeriodInSeconds);
            Assert.AreEqual(BaseTimes.MinimumStorageTiming, runs.MaximumPeriodInSeconds);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RunsSetupNull()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();

            var s = new StorageQueueAutoScaler<object>(count, setup);
            s.Runs(null);
        }

        [Test]
        public void RunnerLow()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            var runs = Substitute.For<IDynamicRuns>();

            var s = new StorageQueueAutoScaler<object>(count, setup);
            var scalable = s.Runner(runs, QueuePriority.Low);

            Assert.IsNotNull(runs);
            Assert.IsNotNull(scalable as AdaptiveRunner);
        }

        [Test]
        public void RunnerMedium()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            var runs = Substitute.For<IDynamicRuns>();

            var s = new StorageQueueAutoScaler<object>(count, setup);
            var scalable = s.Runner(runs, QueuePriority.Medium);

            Assert.IsNotNull(runs);
            Assert.IsNotNull(scalable as BackoffRunner);
        }

        [Test]
        public void RunnerHigh()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            var runs = Substitute.For<IDynamicRuns>();

            var s = new StorageQueueAutoScaler<object>(count, setup);
            var scalable = s.Runner(runs, QueuePriority.High);

            Assert.IsNotNull(runs);
            Assert.IsNotNull(scalable as BackoffRunner);
        }

        [Test]
        public void RunnerHigh()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();

            var s = new StorageQueueAutoScaler<object>(count, setup);
            var unit = s.ScaleUnit(setup);

            Assert.IsNotNull(unit);
            Assert.AreEqual(1, unit.Count());
        }
    }
}
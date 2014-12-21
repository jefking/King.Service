namespace King.Service.Tests.Data
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Scalability;
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class StorageQueueAutoScalerTests
    {
        const string ConnectionString = "UseDevelopmentStorage=true";

        class Setup : QueueSetup<object>
        {
            public override IProcessor<object> Get()
            {
                return Substitute.For<IProcessor<object>>();
            }
        }

        [Test]
        public void Constructor()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            new StorageQueueAutoScaler<object>(count, setup);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThroughputNull()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            new StorageQueueAutoScaler<object>(count, setup, null);
        }

        [Test]
        public void IsQueueAutoScaler()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            Assert.IsNotNull(new StorageQueueAutoScaler<object>(count, setup) as QueueAutoScaler<IQueueSetup<object>>);
        }

        [Test]
        public void Runs()
        {
            var random = new Random();
            var max = (byte)random.Next(byte.MinValue, byte.MaxValue);
            var min = (byte)random.Next(byte.MinValue, max);
            var count = Substitute.For<IQueueCount>();
            var setup = new Setup
            {
                Priority = QueuePriority.High,
                Name = Guid.NewGuid().ToString(),
                ConnectionString = ConnectionString,
            };
            var throughput = Substitute.For<IQueueThroughput>();
            throughput.MinimumFrequency(setup.Priority).Returns(min);
            throughput.MaximumFrequency(setup.Priority).Returns(max);

            var s = new StorageQueueAutoScaler<object>(count, setup, throughput);
            var runs = s.Runs(setup);

            Assert.IsNotNull(runs);
            Assert.AreEqual(min, runs.MinimumPeriodInSeconds);
            Assert.AreEqual(max, runs.MaximumPeriodInSeconds);

            throughput.Received().MinimumFrequency(setup.Priority);
            throughput.Received().MaximumFrequency(setup.Priority);
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
        public void ScaleUnit()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            setup.Name.Returns(Guid.NewGuid().ToString());
            setup.ConnectionString.Returns(ConnectionString);

            var s = new StorageQueueAutoScaler<object>(count, setup);
            var unit = s.ScaleUnit(setup);

            Assert.IsNotNull(unit);
            Assert.AreEqual(1, unit.Count());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScaleUnitSetupNull()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();

            var s = new StorageQueueAutoScaler<object>(count, setup);
            var unit = s.ScaleUnit(null);

            Assert.IsNotNull(unit);
            Assert.AreEqual(1, unit.Count());
        }
    }
}
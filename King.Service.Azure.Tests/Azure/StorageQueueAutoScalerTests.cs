namespace King.Service.Tests.Unit.Data
{
    using global::Azure.Data.Wrappers;
    using King.Service.Azure;
    using System;
    using System.Linq;
    using King.Service.Data;
    using King.Service.Data.Model;
    using King.Service.Scalability;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class StorageQueueAutoScalerTests
    {
        const string ConnectionString = "UseDevelopmentStorage=true";
        
        [Test]
        public void Constructor()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = new QueueConnection<object>();
            new StorageQueueAutoScaler<object>(count, setup);
        }

        [Test]
        public void IsQueueAutoScaler()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = new QueueConnection<object>();
            Assert.IsNotNull(new StorageQueueAutoScaler<object>(count, setup) as QueueAutoScaler<IQueueConnection<object>>);
        }

        [Test]
        public void Runs()
        {
            var random = new Random();
            var frequency = new Range<byte>();
            frequency.Maximum = (byte)random.Next(byte.MinValue, byte.MaxValue);
            frequency.Minimum = (byte)random.Next(byte.MinValue, frequency.Maximum);
            var count = Substitute.For<IQueueCount>();
            var setup = new QueueSetup<object>()
            {
                Priority = QueuePriority.High,
                Name = Guid.NewGuid().ToString(),
                Processor = () => { return Substitute.For<IProcessor<object>>(); },
            };
            var connection = new QueueConnection<object>()
            {
                Setup = setup,
                ConnectionString = ConnectionString,
            };
            var throughput = Substitute.For<IQueueThroughput>();
            throughput.Frequency(setup.Priority).Returns(frequency);

            var s = new StorageQueueAutoScaler<object>(count, connection, throughput);
            var runs = s.Runs(connection);

            Assert.IsNotNull(runs);
            Assert.AreEqual(frequency.Minimum, runs.MinimumPeriodInSeconds);
            Assert.AreEqual(frequency.Maximum, runs.MaximumPeriodInSeconds);

            throughput.Received().Frequency(setup.Priority);
        }

        [Test]
        public void RunsSetupNull()
        {
            var count = Substitute.For<IQueueCount>();

            var s = new StorageQueueAutoScaler<object>(count, new QueueConnection<object>());
            Assert.That(() => s.Runs(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ScaleUnit()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            setup.Name.Returns(Guid.NewGuid().ToString());
            var connection = Substitute.For<IQueueConnection<object>>();
            connection.ConnectionString.Returns(ConnectionString);
            connection.Setup.Returns(setup);

            var s = new StorageQueueAutoScaler<object>(count, connection);
            var unit = s.ScaleUnit(connection);

            Assert.IsNotNull(unit);
            Assert.AreEqual(1, unit.Count());
        }

        [Test]
        public void ScaleUnitSetupNull()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = new QueueConnection<object>();

            var s = new StorageQueueAutoScaler<object>(count, setup);
            var unit = s.ScaleUnit(null);

            Assert.IsNotNull(unit);

            Assert.That(() => unit.Count(), Throws.TypeOf<ArgumentNullException>());
        }
    }
}
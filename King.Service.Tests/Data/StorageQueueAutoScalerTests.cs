namespace King.Service.Tests.Data
{
    using System;
    using System.Linq;
    using King.Azure.Data;
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
            var max = (byte)random.Next(byte.MinValue, byte.MaxValue);
            var min = (byte)random.Next(byte.MinValue, max);
            var count = Substitute.For<IQueueCount>();
            var setup = new QueueSetup<object>()
            {
                Priority = QueuePriority.High,
                Name = Guid.NewGuid().ToString(),
            };
            var connection = new QueueConnection<object>()
            {
                Setup = setup,
                ConnectionString = ConnectionString,
            };
            var throughput = Substitute.For<IQueueThroughput>();
            throughput.MinimumFrequency(setup.Priority).Returns(min);
            throughput.MaximumFrequency(setup.Priority).Returns(max);

            var s = new StorageQueueAutoScaler<object>(count, connection, throughput);
            var runs = s.Runs(connection);

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

            var s = new StorageQueueAutoScaler<object>(count, new QueueConnection<object>());
            s.Runs(null);
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScaleUnitSetupNull()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = new QueueConnection<object>();

            var s = new StorageQueueAutoScaler<object>(count, setup);
            var unit = s.ScaleUnit(null);

            Assert.IsNotNull(unit);
            Assert.AreEqual(1, unit.Count());
        }
    }
}
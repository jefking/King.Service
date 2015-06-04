namespace King.Service.Tests.Data
{
    using System;
    using System.Linq;
    using King.Service.Data;
    using King.Service.Scalability;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class StorageDequeueFactoryTests
    {
        const string ConnectionString = "UseDevelopmentStorage=true";
        
        [Test]
        public void Constructor()
        {
            new StorageDequeueFactory(ConnectionString);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorConnectionStringNull()
        {
            new StorageDequeueFactory(null);
        }

        [Test]
        public void ConstructorThroughputNull()
        {
            new StorageDequeueFactory(ConnectionString, null);
        }
        
        [Test]
        public void Tasks()
        {
            var setup = new QueueSetup<object>()
            {
                Name = "test",
                Priority = QueuePriority.Low,
            };
            var f = new StorageDequeueFactory(ConnectionString);
            var tasks = f.Tasks(setup);

            Assert.IsNotNull(tasks);
            Assert.AreEqual(2, tasks.Count());

            var t = (from n in tasks
                     where n.GetType() == typeof(InitializeStorageTask)
                     select true).FirstOrDefault();

            Assert.IsTrue(t);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TasksSetupNull()
        {
            var f = new StorageDequeueFactory(ConnectionString);
            var tasks = f.Tasks<object>(null);

            Assert.IsNotNull(tasks);
            Assert.AreEqual(2, tasks.Count());
        }
        
        [Test]
        public void DequeueTask()
        {
            var setup = new QueueSetup<object>()
            {
                Name = "test",
                Priority = QueuePriority.High,
            };

            var random = new Random();
            var max = (byte)random.Next(byte.MinValue, byte.MaxValue);
            var min = (byte)random.Next(byte.MinValue, max);
            
            var throughput = Substitute.For<IQueueThroughput>();
            throughput.MaximumScale(setup.Priority).Returns(max);
            throughput.MinimumScale(setup.Priority).Returns(min);

            var f = new StorageDequeueFactory(ConnectionString, throughput);
            var task = f.Dequeue<object>(setup);

            Assert.IsNotNull(task);
            var scaler = task as StorageQueueAutoScaler<object>;
            Assert.IsNotNull(scaler);
            Assert.AreEqual(min, scaler.Minimum);
            Assert.AreEqual(max, scaler.Maximum);

            throughput.Received().MaximumScale(setup.Priority);
            throughput.Received().MinimumScale(setup.Priority);
        }
    
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DequeueTaskSetupNull()
        {
            var df = new StorageDequeueFactory(ConnectionString);
            df.Dequeue<object>(null);
        }
    }
}
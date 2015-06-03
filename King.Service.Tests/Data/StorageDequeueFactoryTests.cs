namespace King.Service.Tests.Data
{
    using System;
    using System.Linq;
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Scalability;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class StorageDequeueFactoryTests
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
            new StorageDequeueFactory<object>(ConnectionString);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorConnectionStringNull()
        {
            new StorageDequeueFactory<object>(null);
        }

        [Test]
        public void IsDequeueFactory()
        {
            Assert.IsNotNull(new StorageDequeueFactory<object>(ConnectionString) as DequeueFactory<object>);
        }

        [Test]
        public void IsITaskFactory()
        {
            Assert.IsNotNull(new StorageDequeueFactory<object>(ConnectionString) as ITaskFactory<IQueueSetup<object>>);
        }

        [Test]
        public void Tasks()
        {
            var setup = new Setup
            {
                ConnectionString = ConnectionString,
                Name = "test",
                Priority = QueuePriority.Low,
            };
            var f = new StorageDequeueFactory<object>(ConnectionString);
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
            var f = new StorageDequeueFactory<object>(ConnectionString);
            var tasks = f.Tasks(null);

            Assert.IsNotNull(tasks);
            Assert.AreEqual(2, tasks.Count());
        }
        
        [Test]
        public void DequeueTask()
        {
            var setup = new Setup
            {
                ConnectionString = ConnectionString,
                Name = "test",
                Priority = QueuePriority.High,
            };

            var random = new Random();
            var max = (byte)random.Next(byte.MinValue, byte.MaxValue);
            var min = (byte)random.Next(byte.MinValue, max);

            var queue = Substitute.For<IStorageQueue>();
            var throughput = Substitute.For<IQueueThroughput>();
            throughput.MaximumScale(setup.Priority).Returns(max);
            throughput.MinimumScale(setup.Priority).Returns(min);

            var f = new StorageDequeueFactory<object>(ConnectionString, throughput);
            var task = f.DequeueTask(queue, setup);

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
        public void DequeueTaskQueueNull()
        {
            var f = new StorageDequeueFactory<object>(ConnectionString);
            var task = f.DequeueTask(null, new Setup());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DequeueTaskSetupNull()
        {
            var queue = Substitute.For<IStorageQueue>();

            var df = new StorageDequeueFactory<object>(ConnectionString);
            var task = df.DequeueTask(queue, null);
        }

        [Test]
        public void Scalable()
        {
            var queue = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();
            
            var df = new StorageDequeueFactory<object>(ConnectionString);
            var t = df.Scalable(queue, processor);

            Assert.IsNotNull(t);
            Assert.IsNotNull(t as QueueSimplifiedScaler);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ScalableQueueNull()
        {
            var processor = Substitute.For<IProcessor<object>>();

            var df = new StorageDequeueFactory<object>(ConnectionString);
            var t = df.Scalable(null, processor);

            Assert.IsNotNull(t);
            Assert.IsNotNull(t as QueueSimplifiedScaler);
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScalableProcessorNull()
        {
            var queue = Guid.NewGuid().ToString();

            var df = new StorageDequeueFactory<object>(ConnectionString);
            var t = df.Scalable(queue, null);

            Assert.IsNotNull(t);
            Assert.IsNotNull(t as QueueSimplifiedScaler);
        }
    }
}
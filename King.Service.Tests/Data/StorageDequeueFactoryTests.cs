namespace King.Service.Tests.Data
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Data.Model;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Linq;

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
            new StorageDequeueFactory<object>();
        }

        [Test]
        public void IsITaskFactory()
        {
            Assert.IsNotNull(new StorageDequeueFactory<object>() as ITaskFactory<IQueueSetup<object>>);
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
            var f = new StorageDequeueFactory<object>();
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
            var f = new StorageDequeueFactory<object>();
            var tasks = f.Tasks(null);

            Assert.IsNotNull(tasks);
            Assert.AreEqual(2, tasks.Count());
        }

        [Test]
        public void DequeueTaskLow()
        {
            var queue = Substitute.For<IStorageQueue>();
            var setup = new Setup
            {
                ConnectionString = ConnectionString,
                Name = "test",
                Priority = QueuePriority.Low,
            };
            var f = new StorageDequeueFactory<object>();
            var task = f.DequeueTask(queue, setup);

            Assert.IsNotNull(task);
            var scaler = task as StorageQueueAutoScaler<object>;
            Assert.IsNotNull(scaler);
            Assert.AreEqual(1, scaler.Minimum);
            Assert.AreEqual(2, scaler.Maximum);
        }

        [Test]
        public void DequeueTaskMedium()
        {
            var queue = Substitute.For<IStorageQueue>();
            var setup = new Setup
            {
                ConnectionString = ConnectionString,
                Name = "test",
                Priority = QueuePriority.Medium,
            };
            var f = new StorageDequeueFactory<object>();
            var task = f.DequeueTask(queue, setup);

            Assert.IsNotNull(task);
            var scaler = task as StorageQueueAutoScaler<object>;
            Assert.IsNotNull(scaler);
            Assert.AreEqual(1, scaler.Minimum);
            Assert.AreEqual(5, scaler.Maximum);
        }

        [Test]
        public void DequeueTaskHigh()
        {
            var queue = Substitute.For<IStorageQueue>();
            var setup = new Setup
            {
                ConnectionString = ConnectionString,
                Name = "test",
                Priority = QueuePriority.High,
            };
            var f = new StorageDequeueFactory<object>();
            var task = f.DequeueTask(queue, setup);

            Assert.IsNotNull(task);
            var scaler = task as StorageQueueAutoScaler<object>;
            Assert.IsNotNull(scaler);
            Assert.AreEqual(2, scaler.Minimum);
            Assert.AreEqual(10, scaler.Maximum);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DequeueTaskQueueNull()
        {
            var setup = new Setup
            {
                ConnectionString = ConnectionString,
                Name = "test",
                Priority = QueuePriority.Low,
            };
            var f = new StorageDequeueFactory<object>();
            var task = f.DequeueTask(null, setup);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DequeueTaskSetupNull()
        {
            var queue = Substitute.For<IStorageQueue>();
            var f = new StorageDequeueFactory<object>();
            var task = f.DequeueTask(queue, null);
        }
    }
}
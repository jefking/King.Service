namespace King.Service.Tests.Unit.Data
{
    using Azure;
    using global::Azure.Data.Wrappers;
    using King.Service.Data;
    using King.Service.Scalability;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class DequeueFactoryTests
    {
        const string ConnectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            new DequeueFactory(ConnectionString);
        }

        [Test]
        public void ConstructorConnectionStringNull()
        {
            Assert.That(() => new DequeueFactory(null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorThroughputNull()
        {
            new DequeueFactory(ConnectionString, null);
        }

        [Test]
        public void IsIDequeueFactory()
        {
            Assert.IsNotNull(new DequeueFactory(ConnectionString) as IDequeueFactory);
        }

        [Test]
        public void InitializeNamesNull()
        {
            var f = new DequeueFactory(ConnectionString);
            var tasks = f.Initialize((string[])null);

            Assert.IsNotNull(tasks);

            Assert.That(() => tasks.Count(), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void InitializeNames()
        {
            var f = new DequeueFactory(ConnectionString);
            var tasks = f.Initialize(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            Assert.IsNotNull(tasks);
            Assert.AreEqual(3, tasks.Count());
        }

        [Test]
        public void InitializeNameNull()
        {
            var f = new DequeueFactory(ConnectionString);
            Assert.That(() => f.Initialize((string)null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void InitializeName()
        {
            var f = new DequeueFactory(ConnectionString);
            var task = f.Initialize(Guid.NewGuid().ToString());

            Assert.IsNotNull(task);
            Assert.IsNotNull(task as InitializeStorageTask);
        }

        [Test]
        public void Tasks()
        {
            var setup = new QueueSetup<object>()
            {
                Name = "test",
                Priority = QueuePriority.Low,
            };
            var f = new DequeueFactory(ConnectionString);
            var tasks = f.Tasks(setup);

            Assert.IsNotNull(tasks);
            Assert.AreEqual(2, tasks.Count());

            var t = (from n in tasks
                     where n.GetType() == typeof(InitializeStorageTask)
                     select true).FirstOrDefault();

            Assert.IsTrue(t);
        }

        [Test]
        public void TasksMultiple()
        {
            var random = new Random();
            var count = random.Next(1, 20);
            var setups = new List<IQueueSetup<object>>();
            for (var i = 0; i < count; i++)
            {
                var setup = new QueueSetup<object>()
                {
                    Name = "test",
                    Priority = QueuePriority.Low,
                };

                setups.Add(setup);
            }
            var f = new DequeueFactory(ConnectionString);
            var tasks = f.Tasks(setups);

            Assert.IsNotNull(tasks);
            Assert.AreEqual(count * 2, tasks.Count());
        }

        [Test]
        public void TasksSetupNull()
        {
            var f = new DequeueFactory(ConnectionString);
            var tasks = f.Tasks<object>((QueueSetup<object>)null);

            Assert.IsNotNull(tasks);

            Assert.That(() => tasks.Count(), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void TasksSetupsNull()
        {
            var f = new DequeueFactory(ConnectionString);
            var tasks = f.Tasks<object>((IEnumerable<QueueSetup<object>>)null);

            Assert.IsNotNull(tasks);

            Assert.That(() => tasks.Count(), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Dequeue()
        {
            var setup = new QueueSetup<object>()
            {
                Name = "test",
                Priority = QueuePriority.High,
            };

            var random = new Random();
            var scale = new Range<byte>();

            scale.Maximum = (byte)random.Next(byte.MinValue, byte.MaxValue);
            scale.Minimum = (byte)random.Next(byte.MinValue, scale.Maximum);

            var throughput = Substitute.For<IQueueThroughput>();
            throughput.Scale(setup.Priority).Returns(scale);
            throughput.CheckScaleEvery(setup.Priority).Returns((byte)random.Next(1, 300));

            var f = new DequeueFactory(ConnectionString, throughput);
            var task = f.Dequeue<object>(setup);

            Assert.IsNotNull(task);
            var scaler = task as StorageQueueAutoScaler<object>;
            Assert.IsNotNull(scaler);
            Assert.AreEqual(scale.Minimum, scaler.InstanceCount.Minimum);
            Assert.AreEqual(scale.Maximum, scaler.InstanceCount.Maximum);

            throughput.Received().Scale(setup.Priority);
            throughput.Received().CheckScaleEvery(setup.Priority);
        }

        [Test]
        public void DequeueTaskSetupNull()
        {
            var df = new DequeueFactory(ConnectionString);
            Assert.That(() => df.Dequeue<object>(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void TasksSimple()
        {
            var f = new DequeueFactory(ConnectionString);
            var tasks = f.Tasks<object>("test", () => { return null; });

            Assert.IsNotNull(tasks);
            Assert.AreEqual(2, tasks.Count());

            var t = (from n in tasks
                     where n.GetType() == typeof(InitializeStorageTask)
                     select true).FirstOrDefault();

            Assert.IsTrue(t);
        }

        [Test]
        public void TasksSimpleQueueNameNull()
        {
            var f = new DequeueFactory(ConnectionString);
            Assert.That(() => f.Tasks<object>(null, () => { return null; }), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void TasksSimpleProcessorNull()
        {
            var f = new DequeueFactory(ConnectionString);
            Assert.That(() => f.Tasks<object>("test", null), Throws.TypeOf<ArgumentNullException>());
        }

        class HelpP : IProcessor<object>
        {
            public Task<bool> Process(object data)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void DequeueCreationDefault()
        {
            var f = new DequeueFactory(ConnectionString);
            var tasks = f.Dequeue<HelpP, object>("testing");

            Assert.IsNotNull(tasks);
            Assert.AreEqual(2, tasks.Count());

            var t = (from n in tasks
                     where n.GetType() == typeof(InitializeStorageTask)
                     select true).FirstOrDefault();

            Assert.IsTrue(t);
        }

        [Test]
        public void DequeueNameNull()
        {
            var f = new DequeueFactory(ConnectionString);
            Assert.That(() => f.Dequeue<HelpP, object>(null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ShardsNameNull()
        {
            var f = new DequeueFactory(ConnectionString);
            Assert.That(() => f.Shards<HelpP, object>(null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ShardsCreationDefault()
        {
            var f = new DequeueFactory(ConnectionString);
            var tasks = f.Shards<HelpP, object>("testing");

            Assert.IsNotNull(tasks);
            Assert.AreEqual(2 * 2, tasks.Count());

            var t = (from n in tasks
                     where n.GetType() == typeof(InitializeStorageTask)
                     select true).FirstOrDefault();

            Assert.IsTrue(t);
        }

        [Test]
        public void ShardsZeroCount()
        {
            var f = new DequeueFactory(ConnectionString);
            var tasks = f.Shards<HelpP, object>("testing", 0);

            Assert.IsNotNull(tasks);
            Assert.AreEqual(2 * 2, tasks.Count());

            var t = (from n in tasks
                     where n.GetType() == typeof(InitializeStorageTask)
                     select true).FirstOrDefault();

            Assert.IsTrue(t);
        }
    }
}
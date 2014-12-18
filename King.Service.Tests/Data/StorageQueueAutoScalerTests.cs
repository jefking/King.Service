namespace King.Service.Tests.Data
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Data.Model;
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
            var setup = new Setup();
            var f = new StorageDequeueFactory<object>();
            var tasks = f.Tasks(setup);

            Assert.IsNotNull(tasks);
            Assert.AreEqual(2, tasks);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TasksSetupNull()
        {
            var f = new StorageDequeueFactory<object>();
            f.Tasks(null);
        }
    }
}
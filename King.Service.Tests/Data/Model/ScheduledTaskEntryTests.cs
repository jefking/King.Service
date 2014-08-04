namespace King.Service.Tests.Data.Model
{
    using King.Service.Data.Model;
    using NUnit.Framework;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;

    [TestFixture]
    public class ScheduledTaskEntryTests
    {
        [Test]
        public void Constructor()
        {
            new ScheduledTaskEntry();
        }

        [Test]
        public void ConstructorServiceName()
        {
            new ScheduledTaskEntry(Guid.NewGuid().ToString());
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ConstructorTypeNull()
        {
            new ScheduledTaskEntry((Type)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorServiceNameNull()
        {
            new ScheduledTaskEntry((string)null);
        }

        [Test]
        public void ConstructorType()
        {
            new ScheduledTaskEntry(this.GetType());
        }

        [Test]
        public void IsTableEntity()
        {
            Assert.IsNotNull(new ScheduledTaskEntry() as TableEntity);
        }

        [Test]
        public void PartitionKey()
        {
            var expected = ScheduledTaskEntry.GenerateLogsPartitionKey(this.GetType().ToString());
            var entity = new ScheduledTaskEntry(this.GetType());
            Assert.AreEqual(expected, entity.PartitionKey);
        }

        [Test]
        public void Identifier()
        {
            var entity = new ScheduledTaskEntry(this.GetType());
            Assert.IsNull(entity.Identifier);
            var data = Guid.NewGuid();
            entity.Identifier = data;
            Assert.AreEqual(data, entity.Identifier);
            entity.Identifier = null;
            Assert.IsNull(entity.Identifier);
        }
    }
}
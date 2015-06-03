namespace King.Service.Tests.Data.Model
{
    using King.Service.Data.Model;
    using Microsoft.WindowsAzure.Storage.Table;
    using NUnit.Framework;
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
        public void IsTableEntity()
        {
            Assert.IsNotNull(new ScheduledTaskEntry() as TableEntity);
        }

        [Test]
        public void PartitionKey()
        {
            var expected = ScheduledTaskEntry.GenerateLogsPartitionKey(this.GetType().ToString());
            var entity = new ScheduledTaskEntry
            {
                PartitionKey = ScheduledTaskEntry.GenerateLogsPartitionKey(this.GetType().ToString()),
                ServiceName = this.GetType().ToString(),
            };

            Assert.AreEqual(expected, entity.PartitionKey);
        }

        [Test]
        public void GenerateLogsPartitionKey()
        {
            var serviceName = Guid.NewGuid().ToString();

            Assert.AreEqual(string.Format("{0}-{1:yyyy}-{1:MM}", serviceName, DateTime.UtcNow), ScheduledTaskEntry.GenerateLogsPartitionKey(serviceName));
        }

        [Test]
        public void Identifier()
        {
            var entity = new ScheduledTaskEntry
            {
                PartitionKey = ScheduledTaskEntry.GenerateLogsPartitionKey(this.GetType().ToString()),
                ServiceName = this.GetType().ToString(),
            };
            Assert.IsNull(entity.Identifier);
            var data = Guid.NewGuid();
            entity.Identifier = data;
            Assert.AreEqual(data, entity.Identifier);
            entity.Identifier = null;
            Assert.IsNull(entity.Identifier);
        }

        [Test]
        public void ServiceName()
        {
            var expected = ScheduledTaskEntry.GenerateLogsPartitionKey(this.GetType().ToString());
            var entity = new ScheduledTaskEntry
            {
                ServiceName = expected,
            };

            Assert.AreEqual(expected, entity.ServiceName);
        }
    }
}
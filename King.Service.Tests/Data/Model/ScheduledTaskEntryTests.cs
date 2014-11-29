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
    }
}
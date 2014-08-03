namespace King.Azure.BackgroundWorker.Integration
{
    using King.Azure.BackgroundWorker.Data;
    using NUnit.Framework;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class TableStorageTests
    {
        private readonly string ConnectionString = "UseDevelopmentStorage=true;";
        ITableStorage storage = null;

        [SetUp]
        public void Init()
        {
            var table = 'a' + Guid.NewGuid().ToString().ToLowerInvariant().Replace('-', 'a');
            this.storage = new TableStorage(table, ConnectionString);
            storage.CreateIfNotExists().Wait();
        }
        
        [TearDown]
        public void Dispose()
        {
            storage.Delete().Wait();
        }

        [Test]
        public async Task CreateIfNotExists()
        {
            var table = 'a' + Guid.NewGuid().ToString().ToLowerInvariant().Replace('-', 'a');
            var storage = new TableStorage(table, ConnectionString);
            var created = await storage.CreateIfNotExists();

            Assert.IsTrue(created);
        }

        [Test]
        public async Task CreateIfNotExistsAlreadyExists()
        {
            var table = 'a' + Guid.NewGuid().ToString().ToLowerInvariant().Replace('-', 'a');
            var storage = new TableStorage(table, ConnectionString);
            var created = await storage.CreateIfNotExists();

            Assert.IsTrue(created);
            created = await storage.CreateIfNotExists();
            Assert.IsFalse(created);
        }

        [Test]
        public async Task Create()
        {
            var table = 'a' + Guid.NewGuid().ToString().ToLowerInvariant().Replace('-', 'a');
            var storage = new TableStorage(table, ConnectionString);
            var created = await storage.Create();

            Assert.IsTrue(created);
        }

        [Test]
        public async Task Delete()
        {
            var table = 'a' + Guid.NewGuid().ToString().ToLowerInvariant().Replace('-', 'a');
            var storage = new TableStorage(table, ConnectionString);
            var created = await storage.Create();

            Assert.IsTrue(created);

            await storage.Delete();

            created = await storage.Create();
            Assert.IsTrue(created);
        }

        [Test]
        public async Task Insert()
        {
            var entity = new TableEntity()
            {
                PartitionKey = "partition",
                RowKey = "row",
            };
            var entities = new List<TableEntity>();
            entities.Add(entity);
            await storage.Insert(entities);

            var returned = storage.QueryByPartition<TableEntity>("partition");
            Assert.IsNotNull(returned);
            Assert.AreEqual(1, returned.Count());
            var e = returned.First();
            Assert.AreEqual(entity.PartitionKey, e.PartitionKey);
            Assert.AreEqual(entity.RowKey, e.RowKey);
        }

        [Test]
        public async Task InsertOrReplace()
        {
            var entity = new TableEntity()
            {
                PartitionKey = "partition",
                RowKey = "row",
            };
            var entities = new List<TableEntity>();
            entities.Add(entity);
            await storage.Insert(entities);
            await storage.InsertOrReplace(entity);

            var returned = storage.QueryByPartition<TableEntity>("partition");
            Assert.IsNotNull(returned);
            Assert.AreEqual(1, returned.Count());
            var e = returned.First();
            Assert.AreEqual(entity.PartitionKey, e.PartitionKey);
            Assert.AreEqual(entity.RowKey, e.RowKey);
        }
    }
}

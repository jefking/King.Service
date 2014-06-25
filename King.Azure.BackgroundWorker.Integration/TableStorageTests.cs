namespace King.Azure.BackgroundWorker.Integration
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using King.Azure.BackgroundWorker.Data;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class TableStorageTests
    {
        private readonly string ConnectionString = "UseDevelopmentStorage=true;";

        [TestMethod]
        public async Task CreateIfNotExists()
        {
            var table = 'a' + Guid.NewGuid().ToString().ToLowerInvariant().Replace('-', 'a');
            var storage = new TableStorage(table, ConnectionString);
            var created = await storage.CreateIfNotExists();

            Assert.IsTrue(created);
        }

        [TestMethod]
        public async Task CreateIfNotExistsAlreadyExists()
        {
            var table = 'a' + Guid.NewGuid().ToString().ToLowerInvariant().Replace('-', 'a');
            var storage = new TableStorage(table, ConnectionString);
            var created = await storage.CreateIfNotExists();

            Assert.IsTrue(created);
            created = await storage.CreateIfNotExists();
            Assert.IsFalse(created);
        }

        [TestMethod]
        public async Task Create()
        {
            var table = 'a' + Guid.NewGuid().ToString().ToLowerInvariant().Replace('-', 'a');
            var storage = new TableStorage(table, ConnectionString);
            var created = await storage.Create();

            Assert.IsTrue(created);
        }

        [TestMethod]
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

        [TestMethod]
        public async Task Insert()
        {
            var table = 'a' + Guid.NewGuid().ToString().ToLowerInvariant().Replace('-', 'a');
            var storage = new TableStorage(table, ConnectionString);
            var created = await storage.Create();

            Assert.IsTrue(created);

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
            Assert.AreEqual<int>(1, returned.Count());
            var e = returned.First();
            Assert.AreEqual<string>(entity.PartitionKey, e.PartitionKey);
            Assert.AreEqual<string>(entity.RowKey, e.RowKey);
        }

        [TestMethod]
        public async Task InsertOrReplace()
        {
            var table = 'a' + Guid.NewGuid().ToString().ToLowerInvariant().Replace('-', 'a');
            var storage = new TableStorage(table, ConnectionString);
            var created = await storage.Create();

            Assert.IsTrue(created);

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
            Assert.AreEqual<int>(1, returned.Count());
            var e = returned.First();
            Assert.AreEqual<string>(entity.PartitionKey, e.PartitionKey);
            Assert.AreEqual<string>(entity.RowKey, e.RowKey);
        }
    }
}

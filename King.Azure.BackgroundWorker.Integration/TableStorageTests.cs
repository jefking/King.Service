namespace King.Azure.BackgroundWorker.Integration
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using King.Azure.BackgroundWorker.Data;

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
        public async Task Create()
        {
            var table = 'a' + Guid.NewGuid().ToString().ToLowerInvariant().Replace('-', 'a');
            var storage = new TableStorage(table, ConnectionString);
            var created = await storage.Create();

            Assert.IsTrue(created);
        }
    }
}

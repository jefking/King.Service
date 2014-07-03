namespace King.Azure.BackgroundWorker.Integration
{
    using King.Azure.BackgroundWorker.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Threading.Tasks;

    [TestClass]
    public class QueueTests
    {
        private readonly string ConnectionString = "UseDevelopmentStorage=true;";

        [TestMethod]
        public async Task CreateIfNotExists()
        {
            var name = 'a' + Guid.NewGuid().ToString().ToLowerInvariant().Replace('-', 'a');
            var storage = new Queue(name, ConnectionString);
            var created = await storage.CreateIfNotExists();

            Assert.IsTrue(created);
        }
    }
}
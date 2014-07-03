namespace King.Azure.BackgroundWorker.Tests.Data
{
    using King.Azure.BackgroundWorker.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class QueueTests
    {
        [TestMethod]
        public void Constructor()
        {
            new Queue("test", "UseDevelopmentStorage=true");
        }

        [TestMethod]
        public void IsIAzureStorage()
        {
            Assert.IsNotNull(new Queue("test", "UseDevelopmentStorage=true") as IAzureStorage);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTableNull()
        {
            new TableStorage(null, "UseDevelopmentStorage=true");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorKeyNull()
        {
            new TableStorage("test", null);
        }

        [TestMethod]
        public void Name()
        {
            var name = Guid.NewGuid().ToString();
            var t = new TableStorage(name, "UseDevelopmentStorage=true");
            Assert.AreEqual(name, t.Name);
        }
    }
}
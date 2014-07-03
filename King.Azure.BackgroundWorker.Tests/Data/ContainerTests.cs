namespace King.Azure.BackgroundWorker.Tests.Data
{
    using King.Azure.BackgroundWorker.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class ContainerTests
    {
        [TestMethod]
        public void Constructor()
        {
            new Container("test", "UseDevelopmentStorage=true");
        }

        [TestMethod]
        public void IsIAzureStorage()
        {
            Assert.IsNotNull(new Container("test", "UseDevelopmentStorage=true") as IAzureStorage);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTableNull()
        {
            new Container(null, "UseDevelopmentStorage=true");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorKeyNull()
        {
            new Container("test", null);
        }

        [TestMethod]
        public void Name()
        {
            var name = Guid.NewGuid().ToString();
            var t = new Container(name, "UseDevelopmentStorage=true");
            Assert.AreEqual(name, t.Name);
        }
    }
}
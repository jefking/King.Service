namespace King.Azure.BackgroundWorker.Tests.Data
{
    using King.Azure.BackgroundWorker.Data;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void Constructor()
        {
            new Container("test", "UseDevelopmentStorage=true");
        }

        [Test]
        public void IsIAzureStorage()
        {
            Assert.IsNotNull(new Container("test", "UseDevelopmentStorage=true") as IAzureStorage);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTableNull()
        {
            new Container(null, "UseDevelopmentStorage=true");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorKeyNull()
        {
            new Container("test", null);
        }

        [Test]
        public void Name()
        {
            var name = Guid.NewGuid().ToString();
            var t = new Container(name, "UseDevelopmentStorage=true");
            Assert.AreEqual(name, t.Name);
        }
    }
}
namespace King.Azure.BackgroundWorker.Tests.Data
{
    using King.Azure.BackgroundWorker.Data;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class QueueTests
    {
        [Test]
        public void Constructor()
        {
            new Queue("test", "UseDevelopmentStorage=true");
        }

        [Test]
        public void IsIAzureStorage()
        {
            Assert.IsNotNull(new Queue("test", "UseDevelopmentStorage=true") as IAzureStorage);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTableNull()
        {
            new TableStorage(null, "UseDevelopmentStorage=true");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorKeyNull()
        {
            new TableStorage("test", null);
        }

        [Test]
        public void Name()
        {
            var name = Guid.NewGuid().ToString();
            var t = new TableStorage(name, "UseDevelopmentStorage=true");
            Assert.AreEqual(name, t.Name);
        }
    }
}
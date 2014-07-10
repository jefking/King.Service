namespace King.Azure.BackgroundWorker.Tests.Data
{
    using King.Azure.BackgroundWorker.Data;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class QueueTests
    {
        private const string ConnectionString = "UseDevelopmentStorage=true;";

        [Test]
        public void Constructor()
        {
            new Queue("test", ConnectionString);
        }

        [Test]
        public void IQueue()
        {
            Assert.IsNotNull(new Queue("test", ConnectionString) as IQueue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTableNull()
        {
            new Queue(null, ConnectionString);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorKeyNull()
        {
            new Queue("test", null);
        }

        [Test]
        public void Name()
        {
            var name = Guid.NewGuid().ToString();
            var t = new Queue(name, ConnectionString);
            Assert.AreEqual(name, t.Name);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task DeleteNull()
        {
            var name = Guid.NewGuid().ToString();
            var t = new Queue(name, ConnectionString);
            await t.Delete(null);
        }
    }
}
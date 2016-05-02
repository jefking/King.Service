namespace King.Service.Tests.Data.Model
{
    using System;
    using King.Service.Data;
    using King.Service.Data.Model;
    using NUnit.Framework;

    [TestFixture]
    public class QueueConnectionTests
    {
        [Test]
        public void Constructor()
        {
            new QueueConnection<object>();
        }

        [Test]
        public void IsIQueueConnection()
        {
            Assert.IsNotNull(new QueueConnection<object>() as IQueueConnection<object>);
        }

        [Test]
        public void ConnectionString()
        {
            var expected = Guid.NewGuid().ToString();
            var qc = new QueueConnection<object>()
            {
                ConnectionString = expected,
            };

            Assert.AreEqual(expected, qc.ConnectionString);
        }

        [Test]
        public void Setup()
        {
            var expected = new QueueSetup<object>();
            var qc = new QueueConnection<object>()
            {
                Setup = expected,
            };

            Assert.AreEqual(expected, qc.Setup);
        }
    }
}
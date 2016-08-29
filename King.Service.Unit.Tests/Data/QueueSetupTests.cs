namespace King.Service.Tests.Data
{
    using System;
    using King.Azure.Data;
    using King.Service.Data;
    using NUnit.Framework;

    [TestFixture]
    public class QueueSetupTests
    {
        [Test]
        public void Constructor()
        {
            new QueueSetup<object>();
        }

        [Test]
        public void IsIQueueSetup()
        {
            Assert.IsNotNull(new QueueSetup<object>() as IQueueSetup<object>);
        }

        [Test]
        public void Name()
        {
            var expected = Guid.NewGuid().ToString();
            var qc = new QueueSetup<object>()
            {
                Name = expected,
            };

            Assert.AreEqual(expected, qc.Name);
        }

        [Test]
        public void Priority()
        {
            var expected = QueuePriority.High;
            var qc = new QueueSetup<object>()
            {
                Priority = expected,
            };

            Assert.AreEqual(expected, qc.Priority);
        }

        [Test]
        public void Processor()
        {
            Func<IProcessor<object>> expected = () => { return null; };
            var qc = new QueueSetup<object>()
            {
                Processor = expected,
            };

            Assert.AreEqual(expected, qc.Processor);
        }
    }
}
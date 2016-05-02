namespace King.Service.Tests.Data
{
    using King.Service.Data;
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class QueueActionTests
    {
        [Test]
        public void Constructor()
        {
            new QueueAction<object>();
        }

        [Test]
        public void IsQueueSetup()
        {
            Assert.IsNotNull(new QueueAction<object>() as QueueSetup<object>);
        }

        [Test]
        public void Activity()
        {
            var expected = new QueueActivity<object>(async (obj) => { return await Task.FromResult<bool>(true); });
            var qc = new QueueAction<object>()
            {
                Activity = expected,
            };

            Assert.AreEqual(expected, qc.Activity);
        }

        [Test]
        public void Processor()
        {
            var expected = new QueueActivity<object>(async (obj) => { return await Task.FromResult<bool>(true); });
            var qc = new QueueAction<object>()
            {
                Activity = expected,
            };

            Assert.IsNotNull(qc.Processor);
        }

        [Test]
        public void ProcessorValue()
        {
            var expected = new QueueActivity<object>(async (obj) => { return await Task.FromResult<bool>(true); });
            var qc = new QueueAction<object>()
            {
                Activity = expected,
            };

            Assert.IsNotNull(qc.Processor() as ActionProcessor<object>);
        }
    }
}
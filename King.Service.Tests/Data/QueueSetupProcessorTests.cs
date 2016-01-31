namespace King.Service.Tests.Data
{
    using System;
    using King.Azure.Data;
    using King.Service.Data;
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class QueueSetupProcessorTests
    {
        #region Classes
        internal class TestProcessor : IProcessor<object>
        {
            public Task<bool> Process(object data)
            {
                throw new NotImplementedException();
            }
        }
        #endregion
        
        [Test]
        public void Constructor()
        {
            new QueueSetupProcessor<TestProcessor, object>();
        }

        [Test]
        public void IsIQueueSetup()
        {
            Assert.IsNotNull(new QueueSetupProcessor<TestProcessor, object>() as QueueSetup<object>);
        }
        
        [Test]
        public void Processor()
        {
            var qsp = new QueueSetupProcessor<TestProcessor, object>();
            var ph = qsp.Processor;
            Assert.IsNotNull(ph);

            var p = ph();
            Assert.IsNotNull(p as TestProcessor);
        }
    }
}
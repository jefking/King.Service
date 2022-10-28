﻿namespace King.Service.Tests.Unit.Data
{
    using global::Azure.Data.Wrappers;
    using King.Service.Data;
    using NUnit.Framework;
    using System;
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
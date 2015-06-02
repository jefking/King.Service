namespace King.Service.Tests.Data
{
    using King.Azure.Data;
    using King.Service.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class QueueSimplifiedScalerTests
    {
        public class MyQScaler : QueueSimplifiedScaler
        {
            public MyQScaler(IQueueCount count)
                : base(count)
            {
            }
        }

        [Test]
        public void Constructor()
        {
            var c = Substitute.For<IQueueCount>();
            new MyQScaler(c);
        }

        [Test]
        public void IsAutoScaler()
        {
            var c = Substitute.For<IQueueCount>();
            Assert.IsNotNull(new MyQScaler(c) as AutoScaler<object>);
        }
    }
}
namespace King.Service.Tests.Unit.Data
{
    using System;
    using System.Collections.Generic;
    using King.Service.Data;
    using King.Service.Scalability;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class QueueAutoScalerTests
    {
        public class MyQScaler : QueueAutoScaler<object>
        {
            public MyQScaler(IQueueCount count)
                :base(count)
            {
            }
            public override IEnumerable<IScalable> ScaleUnit(object data)
            {
                throw new NotImplementedException();
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
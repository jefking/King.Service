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
            public MyQScaler(IQueueCount count, ITaskCreator creator)
                : base(count, creator)
            {
            }
        }

        [Test]
        public void Constructor()
        {
            var count = Substitute.For<IQueueCount>();
            var creator = Substitute.For<ITaskCreator>();
            new MyQScaler(count, creator);
        }

        [Test]
        public void IsAutoScaler()
        {
            var count = Substitute.For<IQueueCount>();
            var creator = Substitute.For<ITaskCreator>();
            Assert.IsNotNull(new MyQScaler(count, creator) as QueueAutoScaler<ITaskCreator>);
        }
    }
}
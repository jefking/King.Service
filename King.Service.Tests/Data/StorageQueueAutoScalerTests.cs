namespace King.Service.Tests.Data
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Data.Model;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class StorageQueueAutoScalerTests
    {
        [Test]
        public void Constructor()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            new StorageQueueAutoScaler<object>(count, setup);
        }

        [Test]
        public void IsQueueAutoScaler()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            Assert.IsNotNull(new StorageQueueAutoScaler<object>(count, setup) as QueueAutoScaler<IQueueSetup<object>>);
        }
    }
}
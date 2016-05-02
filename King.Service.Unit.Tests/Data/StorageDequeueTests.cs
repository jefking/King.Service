namespace King.Service.Tests.Data
{
    using Azure;
    using King.Azure.Data;
    using King.Service.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class StorageDequeueTests
    {
        const string ConnectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            var processor = Substitute.For<IProcessor<object>>();
            new StorageDequeue<object>("queue", ConnectionString, processor);
        }

        [Test]
        public void IsDequeue()
        {
            var processor = Substitute.For<IProcessor<object>>();
            Assert.IsNotNull(new StorageDequeue<object>("queue", ConnectionString, processor) as Dequeue<object>);
        }
    }
}
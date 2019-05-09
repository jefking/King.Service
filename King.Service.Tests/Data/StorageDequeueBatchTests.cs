namespace King.Service.Tests.Data
{
    using Azure;
    using global::Azure.Data.Wrappers;
    using King.Service.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class StorageDequeueBatchTests
    {
        const string ConnectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            var processor = Substitute.For<IProcessor<object>>();
            new StorageDequeueBatch<object>("queue", ConnectionString, processor);
        }

        [Test]
        public void IsDequeueBatch()
        {
            var processor = Substitute.For<IProcessor<object>>();
            Assert.IsNotNull(new StorageDequeueBatch<object>("queue", ConnectionString, processor) as DequeueBatch<object>);
        }
    }
}
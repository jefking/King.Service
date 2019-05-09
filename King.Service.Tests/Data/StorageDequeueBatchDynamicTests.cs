namespace King.Service.Tests.Data
{
    using Azure;
    using global::Azure.Data.Wrappers;
    using King.Service.Data;
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class StorageDequeueBatchDynamicTests
    {
        const string ConnectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            var name = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();
            new StorageDequeueBatchDynamic<object>(name, ConnectionString, processor);
        }

        [Test]
        public void MockableConstructor()
        {
            var queue = Substitute.For<IStorageQueue>();
            var processor = Substitute.For<IProcessor<object>>();
            var tracker = Substitute.For<ITimingTracker>();
            new StorageDequeueBatchDynamic<object>(queue, processor, tracker);
        }

        [Test]
        public void IsDequeueBatchDynamic()
        {
            var queue = Substitute.For<IStorageQueue>();
            var processor = Substitute.For<IProcessor<object>>();
            var tracker = Substitute.For<ITimingTracker>();
            Assert.IsNotNull(new StorageDequeueBatchDynamic<object>(queue, processor, tracker) as DequeueBatchDynamic<object>);
        }

        [Test]
        public void MaxBatchSize()
        {
            Assert.AreEqual(32, StorageDequeueBatchDynamic<object>.MaxBatchSize);
        }

        [Test]
        public void VisibilityDuration()
        {
            Assert.AreEqual(TimeSpan.FromSeconds(45), StorageDequeueBatchDynamic<object>.VisibilityDuration);
        }
    }
}
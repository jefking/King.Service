namespace King.Azure.BackgroundWorker.Tests.Data
{
    using King.Azure.BackgroundWorker.Data;
    using Microsoft.WindowsAzure.Storage.Queue;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class StorageQueuedMessageTests
    {
        [Test]
        public void Constructor()
        {
            var queue = Substitute.For<IStorageQueue>();
            new StorageQueuedMessage<object>(queue, new CloudQueueMessage("ship"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorQueueNull()
        {
            var message = new CloudQueueMessage("ship");
            new StorageQueuedMessage<object>(null, message);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorMessageNull()
        {
            var queue = Substitute.For<IStorageQueue>();
            new StorageQueuedMessage<object>(queue, null);
        }
    }
}
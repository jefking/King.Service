namespace King.Service.Tests.Data
{
    using King.Azure.BackgroundWorker.Data;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class AzureStorageTests
    {
        [Test]
        public void Constructor()
        {
            new AzureStorage("UseDevelopmentStorage=true");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNull()
        {
            new AzureStorage(null);
        }
    }
}
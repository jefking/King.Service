namespace King.Service.Tests.Data.Azure
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using King.Azure.BackgroundWorker.Data.Azure;

    [TestClass]
    public class AzureStorageTests
    {
        [TestMethod]
        public void Constructor()
        {
            new AzureStorage("UseDevelopmentStorage=true");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNull()
        {
            new AzureStorage(null);
        }
    }
}
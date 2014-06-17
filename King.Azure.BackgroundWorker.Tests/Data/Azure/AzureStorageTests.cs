namespace King.Service.Tests.Data.Azure
{
    using King.Azure.BackgroundWorker.Data.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

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
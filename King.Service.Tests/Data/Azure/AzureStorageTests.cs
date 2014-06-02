namespace King.Service.Tests.Data.Azure
{
    using System;
    using King.Service.Data.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
namespace King.Service.Tests.Data.Azure
{
    using System;
    using King.Service.Data.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TableStorageTests
    {
        [TestMethod]
        public void Constructor()
        {
            new TableStorage("TestTable", "TestKey");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTableNull()
        {
            new TableStorage(null, "TestKey");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorKeyNull()
        {
            new TableStorage("TestTable", null);
        }
    }
}
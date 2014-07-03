namespace King.Azure.BackgroundWorker.Tests.Data
{
    using King.Azure.BackgroundWorker.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using System;

    [TestClass]
    public class InitializeStorageTaskTests
    {
        [TestMethod]
        public void Constructor()
        {
            var table = Substitute.For<ITableStorage>();
            new InitializeStorageTask(table);
        }

        [TestMethod]
        public void IsInitializeTask()
        {
            var table = Substitute.For<ITableStorage>();
            Assert.IsNotNull(new InitializeStorageTask(table) as InitializeTask);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTableNull()
        {
            new InitializeStorageTask(null);
        }

        [TestMethod]
        public void Run()
        {
            var table = Substitute.For<ITableStorage>();
            table.CreateIfNotExists();

            var task = new InitializeStorageTask(table);
            task.Run();

            table.Received().CreateIfNotExists();
        }
    }
}
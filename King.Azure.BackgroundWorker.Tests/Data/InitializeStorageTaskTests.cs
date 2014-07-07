namespace King.Azure.BackgroundWorker.Tests.Data
{
    using King.Azure.BackgroundWorker.Data;
    using NUnit.Framework;
    using NSubstitute;
    using System;

    [TestFixture]
    public class InitializeStorageTaskTests
    {
        [Test]
        public void Constructor()
        {
            var table = Substitute.For<ITableStorage>();
            new InitializeStorageTask(table);
        }

        [Test]
        public void IsInitializeTask()
        {
            var table = Substitute.For<ITableStorage>();
            Assert.IsNotNull(new InitializeStorageTask(table) as InitializeTask);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTableNull()
        {
            new InitializeStorageTask(null);
        }

        [Test]
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
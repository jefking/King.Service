namespace King.Service.Tests.Data
{
    using King.Azure.Data;
    using King.Service.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

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

        [Test]
        public void Start()
        {
            var table = Substitute.For<ITableStorage>();
            table.CreateIfNotExists();

            var task = new InitializeStorageTask(table);
            task.Start();

            table.Received().CreateIfNotExists();
        }

        [Test]
        public async Task RunAsync()
        {
            var table = Substitute.For<ITableStorage>();
            table.CreateIfNotExists();

            var task = new InitializeStorageTask(table);
            await task.RunAsync();

            table.Received().CreateIfNotExists();
        }
    }
}
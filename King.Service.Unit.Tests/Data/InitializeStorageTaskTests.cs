namespace King.Service.Tests.Data
{
    using global::Azure.Data.Wrappers;
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
        public void ConstructorTableNull()
        {
            Assert.That(() => new InitializeStorageTask(null), Throws.TypeOf<ArgumentNullException>());
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
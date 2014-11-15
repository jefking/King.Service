namespace King.Service.Tests
{
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class InitializeTaskTests
    {
        [Test]
        public void Constructor()
        {
            new InitializeTask();
        }

        [Test]
        public void IsIRunnable()
        {
            Assert.IsNotNull(new InitializeTask() as IRunnable);
        }

        [Test]
        public void Run()
        {
            var it = new InitializeTask();
            it.Run();
        }

        [Test]
        public async Task RunAsync()
        {
            var it = new InitializeTask();
            await it.RunAsync();
        }

        [Test]
        public void Stop()
        {
            var it = new InitializeTask();
            var result = it.Stop();
            Assert.IsTrue(result);
        }

        [Test]
        public void Start()
        {
            var it = new InitializeTask();
            var result = it.Start();
            Assert.IsTrue(result);
        }

        [Test]
        public void Dispose()
        {
            using (var it = new InitializeTask())
            {
            }
        }
    }
}
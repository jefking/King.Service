namespace King.Service.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class InitializeTaskTests
    {
        public class Helper : InitializeTask
        {
            public override void Run()
            {
            }
        }

        [Test]
        public void Constructor()
        {
            new Helper();
        }

        [Test]
        public void IsTaskManager()
        {
            Assert.IsNotNull(new Helper() as TaskManager);
        }
    }
}
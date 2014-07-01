namespace King.Azure.BackgroundWorker.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class InitializeTaskTests
    {
        public class Helper : InitializeTask
        {
            public override void Run()
            {
            }
        }

        [TestMethod]
        public void Constructor()
        {
            new Helper();
        }

        [TestMethod]
        public void IsTaskManager()
        {
            Assert.IsNotNull(new Helper() as TaskManager);
        }
    }
}
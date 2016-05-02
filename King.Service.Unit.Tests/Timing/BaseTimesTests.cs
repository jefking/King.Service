namespace King.Azure.BackgroundWorker.Tests.Timing
{
    using King.Service.Timing;
    using NUnit.Framework;

    [TestFixture]
    public class BaseTimesTests
    {
        [Test]
        public void ScaleCheck()
        {
            Assert.AreEqual(2, BaseTimes.ScaleCheck);
        }
        
        [Test]
        public void MinimumStorageTiming()
        {
            Assert.AreEqual(10, BaseTimes.DefaultMinimumTiming);
        }
        
        [Test]
        public void MaximumStorageTiming()
        {
            Assert.AreEqual(45, BaseTimes.DefaultMaximumTiming);
        }
    }
}
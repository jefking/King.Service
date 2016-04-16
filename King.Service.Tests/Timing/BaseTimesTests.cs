namespace King.Azure.BackgroundWorker.Tests.Timing
{
    using King.Service.Timing;
    using NUnit.Framework;

    [TestFixture]
    public class BaseTimesTests
    {
        [Test]
        public void NoRepeat()
        {
            Assert.AreEqual(-1, BaseTimes.NoRepeat);
        }
        
        [Test]
        public void ScaleCheck()
        {
            Assert.AreEqual(2, BaseTimes.ScaleCheck);
        }
        
        [Test]
        public void MinimumStorageTiming()
        {
            Assert.AreEqual(10, BaseTimes.MinimumStorageTiming);
        }
        
        [Test]
        public void MaximumStorageTiming()
        {
            Assert.AreEqual(45, BaseTimes.MaximumStorageTiming);
        }
    }
}
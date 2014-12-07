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
        public void MinimumTiming()
        {
            Assert.AreEqual(10, BaseTimes.MinimumTiming);
        }

        [Test]
        public void ScaleCheck()
        {
            Assert.AreEqual(2, BaseTimes.ScaleCheck);
        }
        
        [Test]
        public void MaximumTiming()
        {
            Assert.AreEqual(180, BaseTimes.MaximumTiming);
        }
        
        [Test]
        public void MinimumStorageTiming()
        {
            Assert.AreEqual(15, BaseTimes.MinimumStorageTiming);
        }
        
        [Test]
        public void MaximumStorageTiming()
        {
            Assert.AreEqual(180, BaseTimes.MaximumStorageTiming);
        }
    }
}
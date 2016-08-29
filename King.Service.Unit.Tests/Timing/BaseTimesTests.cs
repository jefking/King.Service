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
        public void DefaultMinimumTiming()
        {
            Assert.AreEqual(10, BaseTimes.DefaultMinimumTiming);
        }

        [Test]
        public void DefaultMaximumTiming()
        {
            Assert.AreEqual(60, BaseTimes.DefaultMaximumTiming);
        }
    }
}
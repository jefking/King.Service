namespace King.Service.Tests.Unit.Timing
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
            Assert.AreEqual(45, BaseTimes.DefaultMaximumTiming);
        }
    }
}
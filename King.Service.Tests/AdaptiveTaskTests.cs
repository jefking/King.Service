namespace King.Service.Tests
{
    using King.Service.Timing;
    using NUnit.Framework;

    [TestFixture]
    public class AdaptiveTaskTests
    {
        [Test]
        public void Constructor()
        {
            using (new AdaptiveHelper())
            {
            }
        }

        [Test]
        public void StartIn()
        {
            using (var t = new AdaptiveHelper())
            {
                Assert.AreEqual(BaseTimes.MinimumTiming, t.StartIn.TotalSeconds);
            }
        }

        [Test]
        public void Every()
        {
            using (var t = new AdaptiveHelper())
            {
                Assert.AreEqual(BaseTimes.MinimumTiming, t.Every.TotalSeconds);
            }
        }
    }
}
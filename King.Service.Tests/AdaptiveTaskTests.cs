namespace King.Service.Tests
{
    using King.Service;
    using King.Service.Timing;
    using NUnit.Framework;

    [TestFixture]
    public class AdaptiveTaskTests
    {
        #region Helper
        public class AdaptiveTest : AdaptiveTask
        {
            public bool Work
            {
                get;
                set;
            }
            public override void Run(out bool workWasDone)
            {
                workWasDone = this.Work;
            }
        }
        #endregion

        [Test]
        public void Constructor()
        {
            using (new AdaptiveTest())
            {
            }
        }

        [Test]
        public void StartIn()
        {
            using (var t = new AdaptiveTest())
            {
                Assert.AreEqual(BaseTimes.MinimumTiming, t.StartIn.TotalSeconds);
            }
        }

        [Test]
        public void Every()
        {
            using (var t = new AdaptiveTest())
            {
                Assert.AreEqual(BaseTimes.MinimumTiming, t.Every.TotalSeconds);
            }
        }
    }
}
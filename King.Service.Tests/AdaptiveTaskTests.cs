namespace King.Service.Tests
{
    using King.Service;
    using NSubstitute;
    using NUnit.Framework;
    using System;

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
                Assert.AreEqual(30, t.StartIn.TotalSeconds);
            }
        }

        [Test]
        public void Every()
        {
            using (var t = new AdaptiveTest())
            {
                Assert.AreEqual(30, t.Every.TotalSeconds);
            }
        }
    }
}
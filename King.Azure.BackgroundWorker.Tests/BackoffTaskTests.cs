namespace King.Azure.BackgroundWorker.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class BackoffTaskTests
    {
        #region Helper
        public class BackoffTest : BackoffTask
        {
            public BackoffTest(int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
                : base(minimumPeriodInSeconds, maximumPeriodInSeconds)
            {
            }
            public BackoffTest(ITiming timing, int minimumPeriodInSeconds = 60, int maximumPeriodInSeconds = 300)
                : base(timing, minimumPeriodInSeconds, minimumPeriodInSeconds)
            {
            }
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
            new BackoffTest();
        }

        [Test]
        public void Constructor()
        {
            new BackoffTest(null);
        }
    }
}

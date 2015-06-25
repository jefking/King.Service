namespace King.Service.Tests
{
    using King.Service;
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BackoffTaskTests
    {
        #region Helper
        public class BackoffTest : BackoffTask
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
            using (new BackoffTest())
            {
            }
        }
    }
}
namespace King.Service.Tests.Timing
{
    using King.Service.Timing;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class TimingTrackerTests
    {
        [Test]
        public void Constructor()
        {
            new TimingTracker(TimeSpan.FromSeconds(10));
        }

        [Test]
        public void IsITimingTracker()
        {
            Assert.IsNotNull(new TimingTracker(TimeSpan.FromSeconds(10)) as ITimingTracker);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMaxZero()
        {
            new TimingTracker(TimeSpan.Zero);
        }
    }
}
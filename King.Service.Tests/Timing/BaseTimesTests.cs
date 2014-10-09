namespace King.Azure.BackgroundWorker.Tests.Timing
{
    using King.Service.Timing;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class BaseTimesTests
    {
        [Test]
        public void NoRepeat()
        {
            Assert.AreEqual(-1, BaseTimes.NoRepeat);
        }
        
        [Test]
        public void InitializationTiming()
        {
            Assert.AreEqual(10, BaseTimes.InitializationTiming);
        }
        
        [Test]
        public void MinimumTiming()
        {
            Assert.AreEqual(15, BaseTimes.MinimumTiming);
        }
        
        [Test]
        public void MaximumTiming()
        {
            Assert.AreEqual(120, BaseTimes.MaximumTiming);
        }
        
        [Test]
        public void MinimumStorageTiming()
        {
            Assert.AreEqual(20, BaseTimes.MinimumStorageTiming);
        }
        
        [Test]
        public void MaximumStorageTiming()
        {
            Assert.AreEqual(240, BaseTimes.MaximumStorageTiming);
        }
    }
}
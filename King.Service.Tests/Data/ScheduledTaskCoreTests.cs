namespace King.Service.Tests.Data
{
    using System;
    using King.Service.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ScheduledTaskCoreTests
    {
        [TestMethod]
        public void Constructor()
        {
            new ScheduledTaskCore(new TimeSpan(9000), "UseDevelopmentStorage=true");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTimeSpanZero()
        {
            new ScheduledTaskCore(new TimeSpan(0), "UseDevelopmentStorage=true");
        }

        [TestMethod]
        public void IsIScheduledTaskCore()
        {
            Assert.IsNotNull(new ScheduledTaskCore(new TimeSpan(10), "UseDevelopmentStorage=true") as IScheduledTaskCore);
        }
    }
}
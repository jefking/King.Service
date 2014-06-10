namespace King.Service.Tests.Data
{
    using King.Azure.BackgroundWorker.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckForTaskEntryNull()
        {
            var core = new ScheduledTaskCore(new TimeSpan(9000), "UseDevelopmentStorage=true");
            core.CheckForTask(null);
        }
    }
}
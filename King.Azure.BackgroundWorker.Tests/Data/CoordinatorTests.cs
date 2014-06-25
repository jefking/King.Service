namespace King.Service.Tests.Data
{
    using King.Azure.BackgroundWorker.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class CoordinatorTests
    {
        [TestMethod]
        public void Constructor()
        {
            new Coordinator(new TimeSpan(9000), "UseDevelopmentStorage=true");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTimeSpanZero()
        {
            new Coordinator(new TimeSpan(0), "UseDevelopmentStorage=true");
        }

        [TestMethod]
        public void IsICoordinator()
        {
            Assert.IsNotNull(new Coordinator(new TimeSpan(10), "UseDevelopmentStorage=true") as ICoordinator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckForTaskEntryNull()
        {
            var core = new Coordinator(new TimeSpan(9000), "UseDevelopmentStorage=true");
            core.Check(null);
        }

        [TestMethod]
        public void InitializeTask()
        {
            var core = new Coordinator(new TimeSpan(9000), "UseDevelopmentStorage=true");
            var init = core.InitializeTask();
            Assert.IsNotNull(init);
            Assert.IsNotNull(init as InitializeTableTask);
        }
    }
}
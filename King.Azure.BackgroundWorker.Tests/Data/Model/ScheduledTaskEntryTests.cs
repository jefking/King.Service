namespace King.Service.Tests.Data.Model
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage.Table;
    using King.Azure.BackgroundWorker.Data.Model;

    [TestClass]
    public class ScheduledTaskEntryTests
    {
        [TestMethod]
        public void Constructor()
        {
            new ScheduledTaskEntry();
        }

        [TestMethod]
        public void ConstructorServiceName()
        {
            new ScheduledTaskEntry(Guid.NewGuid().ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ConstructorTypeNull()
        {
            new ScheduledTaskEntry((Type)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorServiceNameNull()
        {
            new ScheduledTaskEntry((string)null);
        }

        [TestMethod]
        public void ConstructorType()
        {
            new ScheduledTaskEntry(this.GetType());
        }
        [TestMethod]
        public void IsTableEntity()
        {
            Assert.IsNotNull(new ScheduledTaskEntry() as TableEntity);
        }
    }
}

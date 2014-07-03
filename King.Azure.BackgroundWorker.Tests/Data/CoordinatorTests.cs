namespace King.Service.Tests.Data
{
    using King.Azure.BackgroundWorker.Data;
    using King.Azure.BackgroundWorker.Data.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestClass]
    public class CoordinatorTests
    {
        [TestMethod]
        public void Constructor()
        {
            new Coordinator(new TimeSpan(9000), "UseDevelopmentStorage=true");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTableNull()
        {
            var core = new Coordinator(null, new TimeSpan(9000));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTimeSpanZero()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, TimeSpan.Zero);
        }

        [TestMethod]
        public void IsICoordinator()
        {
            var table = Substitute.For<ITableStorage>();
            Assert.IsNotNull(new Coordinator(table, new TimeSpan(9000)) as ICoordinator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckTypeNull()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            core.Check(null);
        }

        [TestMethod]
        public void CheckNoRecords()
        {
            var table = Substitute.For<ITableStorage>();
            table.QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>()).Returns(new List<ScheduledTaskEntry>());

            var core = new Coordinator(table, new TimeSpan(9000));
            var perform = core.Check(this.GetType());

            Assert.IsTrue(perform);

            table.Received().QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>());
        }

        [TestMethod]
        public void CheckCompletedFailure()
        {
            var records = new List<ScheduledTaskEntry>();
            var record = new ScheduledTaskEntry()
            {
                StartTime = DateTime.UtcNow,
                CompletionTime = DateTime.UtcNow.AddHours(-1),
                Successful = false,
            };
            records.Add(record);

            var table = Substitute.For<ITableStorage>();
            table.QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>()).Returns(records);

            var core = new Coordinator(table, new TimeSpan(9000));
            var perform = core.Check(this.GetType());

            Assert.IsTrue(perform);

            table.Received().QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>());
        }

        [TestMethod]
        public void CheckCompletedFailue()
        {
            var records = new List<ScheduledTaskEntry>();
            var record = new ScheduledTaskEntry()
            {
                StartTime = DateTime.UtcNow,
                CompletionTime = DateTime.UtcNow,
                Successful = true,
            };
            records.Add(record);

            var table = Substitute.For<ITableStorage>();
            table.QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>()).Returns(records);

            var core = new Coordinator(table, TimeSpan.FromHours(1));
            var perform = core.Check(this.GetType());

            Assert.IsFalse(perform);

            table.Received().QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>());
        }

        [TestMethod]
        public void Check()
        {
            var records = new List<ScheduledTaskEntry>();
            var record = new ScheduledTaskEntry()
            {
                StartTime = DateTime.UtcNow.AddHours(-1),
                CompletionTime = null,
            };
            records.Add(record);

            var table = Substitute.For<ITableStorage>();
            table.QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>()).Returns(records);

            var core = new Coordinator(table, TimeSpan.FromHours(1));
            var perform = core.Check(this.GetType());

            Assert.IsTrue(perform);

            table.Received().QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>());
        }

        [TestMethod]
        public void InitializeTask()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            var init = core.InitializeTask();
            Assert.IsNotNull(init);
            Assert.IsNotNull(init as InitializeStorageTask);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task StartTypeNull()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            await core.Start(null, Guid.NewGuid(), DateTime.UtcNow);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task StartIdentifierEmpty()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            await core.Start(this.GetType(), Guid.Empty, DateTime.UtcNow);
        }

        [TestMethod]
        public async Task Start()
        {
            var table = Substitute.For<ITableStorage>();
            table.InsertOrReplace(Arg.Any<ScheduledTaskEntry>());

            var core = new Coordinator(table, new TimeSpan(9000));
            await core.Start(this.GetType(), Guid.NewGuid(), DateTime.UtcNow);

            table.Received().InsertOrReplace(Arg.Any<ScheduledTaskEntry>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task CompeleteTypeNull()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            await core.Complete(null, Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CompeleteIdentifierEmpty()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            await core.Complete(this.GetType(), Guid.Empty, DateTime.UtcNow, DateTime.UtcNow, true);
        }

        [TestMethod]
        public async Task Compelete()
        {
            var table = Substitute.For<ITableStorage>();
            table.InsertOrReplace(Arg.Any<ScheduledTaskEntry>());

            var core = new Coordinator(table, new TimeSpan(9000));
            await core.Complete(this.GetType(), Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, true);

            table.Received().InsertOrReplace(Arg.Any<ScheduledTaskEntry>());
        }
    }
}
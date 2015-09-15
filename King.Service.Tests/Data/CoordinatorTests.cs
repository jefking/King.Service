namespace King.Service.Tests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Data.Model;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CoordinatorTests
    {
        [Test]
        public void Constructor()
        {
            new Coordinator(new TimeSpan(9000), "UseDevelopmentStorage=true");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTableNull()
        {
            var core = new Coordinator(null, new TimeSpan(9000));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTimeSpanZero()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, TimeSpan.Zero);
        }

        [Test]
        public void IsICoordinator()
        {
            var table = Substitute.For<ITableStorage>();
            Assert.IsNotNull(new Coordinator(table, new TimeSpan(9000)) as ICoordinator);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task CheckTypeNull()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            await core.Check(null);
        }

        [Test]
        public async Task CheckNoRecords()
        {
            var table = Substitute.For<ITableStorage>();
            table.QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>()).Returns(Task.FromResult<IEnumerable<ScheduledTaskEntry>>(new List<ScheduledTaskEntry>()));

            var core = new Coordinator(table, new TimeSpan(9000));
            var perform = await core.Check(this.GetType());

            Assert.IsTrue(perform);

            table.Received().QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>());
        }

        [Test]
        public async Task CheckCompletedFailureOld()
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
            table.QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>()).Returns(Task.FromResult<IEnumerable<ScheduledTaskEntry>>(records));

            var core = new Coordinator(table, new TimeSpan(9000));
            var perform = await core.Check(this.GetType());

            Assert.IsTrue(perform);

            table.Received().QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>());
        }

        [Test]
        public async Task CheckCompletedFailure()
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
            table.QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>()).Returns(Task.FromResult<IEnumerable<ScheduledTaskEntry>>(records));

            var core = new Coordinator(table, TimeSpan.FromHours(1));
            var perform = await core.Check(this.GetType());

            Assert.IsFalse(perform);

            table.Received().QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>());
        }

        [Test]
        public async Task Check()
        {
            var records = new List<ScheduledTaskEntry>();
            var record = new ScheduledTaskEntry()
            {
                StartTime = DateTime.UtcNow.AddHours(-1),
                CompletionTime = null,
            };
            records.Add(record);

            var table = Substitute.For<ITableStorage>();
            table.QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>()).Returns(Task.FromResult<IEnumerable<ScheduledTaskEntry>>(records));

            var core = new Coordinator(table, TimeSpan.FromSeconds(1));
            var perform = await core.Check(this.GetType());

            Assert.IsTrue(perform);

            table.Received().QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>());
        }

        [Test]
        public void InitializeTask()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            var init = core.InitializeTask();
            Assert.IsNotNull(init);
            Assert.IsNotNull(init as InitializeStorageTask);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task StartTypeNull()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            await core.Start(null, Guid.NewGuid(), DateTime.UtcNow);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public async Task StartIdentifierEmpty()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            await core.Start(this.GetType(), Guid.Empty, DateTime.UtcNow);
        }

        [Test]
        public async Task Start()
        {
            var table = Substitute.For<ITableStorage>();
            table.InsertOrReplace(Arg.Any<ScheduledTaskEntry>());

            var core = new Coordinator(table, new TimeSpan(9000));
            await core.Start(this.GetType(), Guid.NewGuid(), DateTime.UtcNow);

            table.Received().InsertOrReplace(Arg.Any<ScheduledTaskEntry>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task CompeleteTypeNull()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            await core.Complete(null, Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, true);
        }

        [Test]
        public async Task CompeleteIdentifierEmpty()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            await core.Complete(this.GetType(), Guid.Empty, DateTime.UtcNow, DateTime.UtcNow, true);
        }

        [Test]
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
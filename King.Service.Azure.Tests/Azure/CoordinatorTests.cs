namespace King.Service.Tests.Unit.Data
{
    using global::Azure.Data.Wrappers;
    using King.Service.Azure.Model;
    using King.Service.Data;
    using King.Service.Data.Model;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    [TestFixture]
    public class CoordinatorTests
    {
        [Test]
        public void Constructor()
        {
            new Coordinator(new TimeSpan(9000), "UseDevelopmentStorage=true");
        }

        [Test]
        public void ConstructorTableNull()
        {
            Assert.That(() => new Coordinator(null, new TimeSpan(9000)), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorTimeSpanZero()
        {
            var table = Substitute.For<ITableStorage>();
            Assert.That(() => new Coordinator(table, TimeSpan.Zero), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsICoordinator()
        {
            var table = Substitute.For<ITableStorage>();
            Assert.IsNotNull(new Coordinator(table, new TimeSpan(9000)) as ICoordinator);
        }

        [Test]
        public void IsITaskFactory()
        {
            var table = Substitute.For<ITableStorage>();
            Assert.IsNotNull(new Coordinator(table, new TimeSpan(9000)) as ITaskFactory<object>);
        }

        [Test]
        public void CheckTypeNull()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            Assert.That(async () => await core.Check(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task CheckNoRecords()
        {
            var table = Substitute.For<ITableStorage>();
            table.QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>()).Returns(Task.FromResult<IEnumerable<ScheduledTaskEntry>>(new List<ScheduledTaskEntry>()));

            var core = new Coordinator(table, new TimeSpan(9000));
            var perform = await core.Check(this.GetType());

            Assert.IsTrue(perform);

            await table.Received().QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>());
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

            await table.Received().QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>());
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

            await table.Received().QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>());
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

            await table.Received().QueryByPartition<ScheduledTaskEntry>(Arg.Any<string>());
        }

        [Test]
        public void InitializeTask()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            var init = core.Tasks().FirstOrDefault();
            Assert.IsNotNull(init);
            Assert.IsNotNull(init as InitializeStorageTask);
        }

        [Test]
        public void InitializeTaskPassthroughNull()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            var init = core.Tasks(null).FirstOrDefault();
            Assert.IsNotNull(init);
            Assert.IsNotNull(init as InitializeStorageTask);
        }

        [Test]
        public void StartTypeNull()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            Assert.That(async () => await core.Start(null, Guid.NewGuid(), DateTime.UtcNow), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void StartIdentifierEmpty()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            Assert.That(async () => await core.Start(this.GetType(), Guid.Empty, DateTime.UtcNow), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public async Task Start()
        {
            var table = Substitute.For<ITableStorage>();
            await table.InsertOrReplace(Arg.Any<ScheduledTaskEntry>());

            var core = new Coordinator(table, new TimeSpan(9000));
            await core.Start(this.GetType(), Guid.NewGuid(), DateTime.UtcNow);

            await table.Received().InsertOrReplace(Arg.Any<ScheduledTaskEntry>());
        }

        [Test]
        public void CompeleteTypeNull()
        {
            var table = Substitute.For<ITableStorage>();
            var core = new Coordinator(table, new TimeSpan(9000));
            Assert.That(async () => await core.Complete(null, Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, true), Throws.TypeOf<ArgumentNullException>());
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
            await table.InsertOrReplace(Arg.Any<ScheduledTaskEntry>());

            var core = new Coordinator(table, new TimeSpan(9000));
            await core.Complete(this.GetType(), Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, true);

            await table.Received().InsertOrReplace(Arg.Any<ScheduledTaskEntry>());
        }
    }
}
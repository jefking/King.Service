﻿namespace King.Service.Tests.Unit
{
    using King.Service.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class CoordinatedTaskTests
    {
        #region Class
        public class TestManager : CoordinatedTask
        {
            public TestManager()
                : base("UseDevelopmentStorage=true;")
            {
            }
            public TestManager(ICoordinator c)
                : base(c)
            {
            }

            public bool Throw;
            public override void Run()
            {
                if (Throw)
                {
                    throw new Exception();
                }
            }
        }
        #endregion

        [Test]
        public void Constructor()
        {
            new TestManager();
        }

        [Test]
        public void ConstructorCoordinatorNull()
        {
            Assert.That(() => new TestManager(null), Throws.TypeOf<NullReferenceException>());
        }
        
        [Test]
        public void IsITaskFactory()
        {
            Assert.IsNotNull(new TestManager() as ITaskFactory<object>);
        }

        [Test]
        public void InitializeTask()
        {
            var m = new TestManager();
            var t = m.Tasks().FirstOrDefault();
            Assert.IsNotNull(t);
            Assert.IsNotNull(t as InitializeStorageTask);
        }

        [Test]
        public void InitializeTaskPassthroughNull()
        {
            var m = new TestManager();
            var t = m.Tasks(null).FirstOrDefault();
            Assert.IsNotNull(t);
            Assert.IsNotNull(t as InitializeStorageTask);
        }

        [Test]
        public void RunObj()
        {
            var coordinator = Substitute.For<ICoordinator>();
            using (var m = new TestManager(coordinator))
            {
                m.Run();
            }
        }
        
        [Test]
        public async Task RunCheckFalse()
        {
            var coordinator = Substitute.For<ICoordinator>();
            coordinator.PeriodInSeconds.Returns(100);
            coordinator.Check(Arg.Any<Type>()).Returns(Task.FromResult(false));

            using (var m = new TestManager(coordinator))
            {
                await m.RunAsync();
            }

            await coordinator.Received().Check(Arg.Any<Type>());
        }

        [Test]
        public async Task Run()
        {
            var coordinator = Substitute.For<ICoordinator>();
            coordinator.PeriodInSeconds.Returns(100000);
            coordinator.Check(Arg.Any<Type>()).Returns(Task.FromResult(true));
            coordinator.Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>()).Returns(Task.FromResult(0));
            coordinator.Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), true).Returns(Task.FromResult(0));

            using (var m = new TestManager(coordinator))
            {
                await m.RunAsync();
            }

            await coordinator.Received().Check(Arg.Any<Type>());
            await coordinator.Received().Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            await coordinator.Received().Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), true);
        }

        [Test]
        public async Task RunCheckThrows()
        {
            var coordinator = Substitute.For<ICoordinator>();
            coordinator.PeriodInSeconds.Returns(100000);
            coordinator.Check(Arg.Any<Type>()).ReturnsForAnyArgs<object>(x => { throw new Exception(); });
            await coordinator.Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            await coordinator.Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());

            using (var m = new TestManager(coordinator))
            {
                await m.RunAsync();
            }

            await coordinator.Received().Check(Arg.Any<Type>());
            await coordinator.DidNotReceive().Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            await coordinator.DidNotReceive().Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());
        }

        [Test]
        public async Task RunStartThrows()
        {
            var coordinator = Substitute.For<ICoordinator>();
            coordinator.PeriodInSeconds.Returns(100000);
            coordinator.Check(Arg.Any<Type>()).Returns(Task.FromResult(true));
            coordinator.When(c => c.Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>())).Do(x => { throw new Exception(); });
            await coordinator.Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());

            using (var m = new TestManager(coordinator))
            {
                await m.RunAsync();
            }

            await coordinator.Received().Check(Arg.Any<Type>());
            await coordinator.Received().Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            await coordinator.DidNotReceive().Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());
        }

        [Test]
        public async Task RunThrows()
        {
            var coordinator = Substitute.For<ICoordinator>();
            coordinator.PeriodInSeconds.Returns(100000);
            coordinator.Check(Arg.Any<Type>()).Returns(Task.FromResult(true));
            await coordinator.Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            await coordinator.Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());

            using (var m = new TestManager(coordinator))
            {
                m.Throw = true;
                await m.RunAsync();
            }

            await coordinator.Received().Check(Arg.Any<Type>());
            await coordinator.Received().Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            await coordinator.Received().Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());
        }
    }
}
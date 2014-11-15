namespace King.Service.Tests
{
    using King.Service.Data;
    using NUnit.Framework;
    using NSubstitute;
    using System;
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
        [ExpectedException(typeof(NullReferenceException))]
        public void ConstructorCoordinatorNull()
        {
            new TestManager(null);
        }

        [Test]
        public void InitializeTask()
        {
            var m = new TestManager();
            var t = m.InitializeTask();
            Assert.IsNotNull(t);
            Assert.IsNotNull(t as InitializeStorageTask);
        }

        [Test]
        public void RunObj()
        {
            var coordinator = Substitute.For<ICoordinator>();
            using (var m = new TestManager(coordinator))
            {
                m.Run(new object());
            }
        }

        [Test]
        public void RunObjNull()
        {
            var coordinator = Substitute.For<ICoordinator>();
            using (var m = new TestManager(coordinator))
            {
                m.Run(null);
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

            coordinator.Received().Check(Arg.Any<Type>());
        }

        [Test]
        public async Task Run()
        {
            var coordinator = Substitute.For<ICoordinator>();
            coordinator.PeriodInSeconds.Returns(100000000);
            coordinator.Check(Arg.Any<Type>()).Returns(Task.FromResult(true));
            coordinator.Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>()).Returns(Task.FromResult(0));
            coordinator.Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), true).Returns(Task.FromResult(0));

            using (var m = new TestManager(coordinator))
            {
                await m.RunAsync();
            }

            coordinator.Received().Check(Arg.Any<Type>());
            coordinator.Received().Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            coordinator.Received().Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), true);
        }

        [Test]
        public async Task RunCheckThrows()
        {
            var coordinator = Substitute.For<ICoordinator>();
            coordinator.PeriodInSeconds.Returns(100000000);
            coordinator.Check(Arg.Any<Type>()).Returns(x => { throw new Exception(); });
            coordinator.Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            coordinator.Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());

            using (var m = new TestManager(coordinator))
            {
                await m.RunAsync();
            }

            coordinator.Received().Check(Arg.Any<Type>());
            coordinator.DidNotReceive().Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            coordinator.DidNotReceive().Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());
        }

        [Test]
        public async Task RunStartThrows()
        {
            var coordinator = Substitute.For<ICoordinator>();
            coordinator.PeriodInSeconds.Returns(100000000);
            coordinator.Check(Arg.Any<Type>()).Returns(Task.FromResult(true));
            coordinator.When(c => c.Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>())).Do(x => { throw new Exception(); });
            coordinator.Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());

            using (var m = new TestManager(coordinator))
            {
                await m.RunAsync();
            }

            coordinator.Received().Check(Arg.Any<Type>());
            coordinator.Received().Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            coordinator.DidNotReceive().Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());
        }

        [Test]
        public async Task RunThrows()
        {
            var coordinator = Substitute.For<ICoordinator>();
            coordinator.PeriodInSeconds.Returns(100000000);
            coordinator.Check(Arg.Any<Type>()).Returns(Task.FromResult(true));
            coordinator.Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            coordinator.Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());

            using (var m = new TestManager(coordinator))
            {
                m.Throw = true;
                await m.RunAsync();
            }

            coordinator.Received().Check(Arg.Any<Type>());
            coordinator.Received().Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            coordinator.Received().Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());
        }
    }
}
namespace King.Azure.BackgroundWorker.Tests
{
    using King.Azure.BackgroundWorker.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using System;
    using System.Threading.Tasks;

    [TestClass]
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

        [TestMethod]
        public void Constructor()
        {
            new TestManager();
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ConstructorCoordinatorNull()
        {
            new TestManager(null);
        }

        [TestMethod]
        public void InitializeTask()
        {
            var m = new TestManager();
            var t = m.InitializeTask();
            Assert.IsNotNull(t);
            Assert.IsNotNull(t as InitializeStorageTask);
        }

        [TestMethod]
        public void RunCheckFalse()
        {
            var coordinator = Substitute.For<ICoordinator>();
            coordinator.PeriodInSeconds.Returns(100);
            coordinator.Check(Arg.Any<Type>()).Returns(false);

            var m = new TestManager(coordinator);
            m.Run(this);

            coordinator.Received().Check(Arg.Any<Type>());
        }

        [TestMethod]
        public void Run()
        {
            var coordinator = Substitute.For<ICoordinator>();
            coordinator.PeriodInSeconds.Returns(100000000);
            coordinator.Check(Arg.Any<Type>()).Returns(true);
            coordinator.Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>()).Returns(Task.FromResult(0));
            coordinator.Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), true).Returns(Task.FromResult(0));

            using (var m = new TestManager(coordinator))
            {
                m.Run(this);
            }

            coordinator.Received().Check(Arg.Any<Type>());
            coordinator.Received().Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            coordinator.Received().Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), true);
        }

        [TestMethod]
        public void RunCheckThrows()
        {
            var coordinator = Substitute.For<ICoordinator>();
            coordinator.PeriodInSeconds.Returns(100000000);
            coordinator.Check(Arg.Any<Type>()).Returns(x => { throw new Exception(); });
            coordinator.Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            coordinator.Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());

            var m = new TestManager(coordinator);
            m.Run(this);

            coordinator.Received().Check(Arg.Any<Type>());
            coordinator.DidNotReceive().Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            coordinator.DidNotReceive().Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());
        }

        [TestMethod]
        public void RunStartThrows()
        {
            var coordinator = Substitute.For<ICoordinator>();
            coordinator.PeriodInSeconds.Returns(100000000);
            coordinator.Check(Arg.Any<Type>()).Returns(true);
            coordinator.When(c => c.Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>())).Do(x => { throw new Exception(); });
            coordinator.Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());

            var m = new TestManager(coordinator);
            m.Run(this);

            coordinator.Received().Check(Arg.Any<Type>());
            coordinator.Received().Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            coordinator.DidNotReceive().Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());
        }

        [TestMethod]
        public async Task RunThrows()
        {
            var coordinator = Substitute.For<ICoordinator>();
            coordinator.PeriodInSeconds.Returns(100000000);
            coordinator.Check(Arg.Any<Type>()).Returns(true);
            coordinator.Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            coordinator.Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());

            var m = new TestManager(coordinator)
            {
                Throw = true,
            };
            m.Run(this);

            coordinator.Received().Check(Arg.Any<Type>());
            coordinator.Received().Start(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
            coordinator.Received().Complete(Arg.Any<Type>(), Arg.Any<Guid>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<bool>());
        }
    }
}
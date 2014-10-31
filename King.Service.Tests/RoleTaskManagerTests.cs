namespace King.Service.Tests
{
    using NUnit.Framework;
    using NSubstitute;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class RoleTaskManagerTests
    {
        [Test]
        public void Constructor()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            new RoleTaskManager<object>(factory);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorServiceFactoryNull()
        {
            new RoleTaskManager<object>(null);
        }

        [Test]
        public void IsIRoleServiceManager()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            Assert.IsNotNull(new RoleTaskManager<object>(factory) as IRoleServiceManager<object>);
        }

        [Test]
        public void Run()
        {
            var services = new List<IRunnable>();
            var service = Substitute.For<IRunnable>();
            service.Start();

            services.Add(service);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleTaskManager<object>>()).Returns(services);

            var manager = new RoleTaskManager<object>(factory);
            manager.OnStart();
            manager.Run();

            service.Received().Start();
            factory.Received().Tasks(Arg.Any<RoleTaskManager<object>>());
        }

        [Test]
        public void RunThrows()
        {
            var services = new List<IRunnable>();
            var service = Substitute.For<IRunnable>();
            service.When(s => s.Start()).Do(x => { throw new Exception(); });

            services.Add(service);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleTaskManager<object>>()).Returns(services);

            var manager = new RoleTaskManager<object>(factory);
            manager.OnStart();
            manager.Run();

            service.Received().Start();
            factory.Received().Tasks(Arg.Any<RoleTaskManager<object>>());
        }

        [Test]
        public void RunNoServices()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var manager = new RoleTaskManager<object>(factory);
            manager.Run();
        }

        [Test]
        public void OnStart()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var manager = new RoleTaskManager<object>(factory);
            manager.OnStart();
        }

        [Test]
        public void OnStartNoServices()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleTaskManager<object>>()).Returns((IEnumerable<IRunnable>)null);

            var manager = new RoleTaskManager<object>(factory);
            manager.OnStart();

            factory.Received().Tasks(Arg.Any<RoleTaskManager<object>>());
        }

        [Test]
        public void OnStop()
        {
            var services = new List<IRunnable>();
            var service = Substitute.For<IRunnable>();
            service.Stop();

            services.Add(service);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleTaskManager<object>>()).Returns(services);

            var manager = new RoleTaskManager<object>(factory);
            manager.OnStart();
            manager.OnStop();

            service.Received().Stop();
            factory.Received().Tasks(Arg.Any<RoleTaskManager<object>>());
        }

        [Test]
        public void OnStopThrows()
        {
            var services = new List<IRunnable>();
            var service = Substitute.For<IRunnable>();
            service.When(s => s.Stop()).Do(x => { throw new Exception(); });

            services.Add(service);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleTaskManager<object>>()).Returns(services);

            var manager = new RoleTaskManager<object>(factory);
            manager.OnStart();
            manager.OnStop();

            service.Received().Stop();
            factory.Received().Tasks(Arg.Any<RoleTaskManager<object>>());
        }

        [Test]
        public void OnStopNoServices()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var manager = new RoleTaskManager<object>(factory);
            manager.OnStop();
        }
    }
}
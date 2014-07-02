namespace King.Azure.BackgroundWorker.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class RoleTaskManagerTests
    {
        [TestMethod]
        public void Constructor()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            new RoleTaskManager(factory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorServiceFactoryNull()
        {
            new RoleTaskManager(null);
        }

        [TestMethod]
        public void IsIRoleServiceManager()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            Assert.IsNotNull(new RoleTaskManager(factory) as IRoleServiceManager);
        }

        [TestMethod]
        public void Run()
        {
            var services = new List<IRunnable>();
            var service = Substitute.For<IRunnable>();
            service.Start();

            services.Add(service);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleTaskManager>()).Returns(services);

            var manager = new RoleTaskManager(factory);
            manager.OnStart();
            manager.Run();

            service.Received().Start();
            factory.Received().Tasks(Arg.Any<RoleTaskManager>());
        }

        [TestMethod]
        public void RunThrows()
        {
            var services = new List<IRunnable>();
            var service = Substitute.For<IRunnable>();
            service.When(s => s.Start()).Do(x => { throw new Exception(); });

            services.Add(service);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleTaskManager>()).Returns(services);

            var manager = new RoleTaskManager(factory);
            manager.OnStart();
            manager.Run();

            service.Received().Start();
            factory.Received().Tasks(Arg.Any<RoleTaskManager>());
        }

        [TestMethod]
        public void RunNoServices()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var manager = new RoleTaskManager(factory);
            manager.Run();
        }

        [TestMethod]
        public void OnStart()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var manager = new RoleTaskManager(factory);
            manager.OnStart();
        }

        [TestMethod]
        public void OnStartNoServices()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleTaskManager>()).Returns((IEnumerable<IRunnable>)null);
            
            var manager = new RoleTaskManager(factory);
            manager.OnStart();

            factory.Received().Tasks(Arg.Any<RoleTaskManager>());
        }

        [TestMethod]
        public void OnStop()
        {
            var services = new List<IRunnable>();
            var service = Substitute.For<IRunnable>();
            service.Stop();

            services.Add(service);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleTaskManager>()).Returns(services);

            var manager = new RoleTaskManager(factory);
            manager.OnStart();
            manager.OnStop();

            service.Received().Stop();
            factory.Received().Tasks(Arg.Any<RoleTaskManager>());
        }

        [TestMethod]
        public void OnStopThrows()
        {
            var services = new List<IRunnable>();
            var service = Substitute.For<IRunnable>();
            service.When(s => s.Stop()).Do(x => { throw new Exception(); });

            services.Add(service);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleTaskManager>()).Returns(services);

            var manager = new RoleTaskManager(factory);
            manager.OnStart();
            manager.OnStop();

            service.Received().Stop();
            factory.Received().Tasks(Arg.Any<RoleTaskManager>());
        }

        [TestMethod]
        public void OnStopNoServices()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var manager = new RoleTaskManager(factory);
            manager.OnStop();
        }
    }
}
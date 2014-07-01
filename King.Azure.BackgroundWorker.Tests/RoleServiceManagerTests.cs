namespace King.Azure.BackgroundWorker.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class RoleServiceManagerTests
    {
        [TestMethod]
        public void Constructor()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            new RoleServiceManager(factory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorServiceFactoryNull()
        {
            new RoleServiceManager(null);
        }

        [TestMethod]
        public void IsIRoleServiceManager()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            Assert.IsNotNull(new RoleServiceManager(factory) as IRoleServiceManager);
        }

        [TestMethod]
        public void Run()
        {
            var services = new List<IRunnable>();
            var service = Substitute.For<IRunnable>();
            service.Start();

            services.Add(service);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleServiceManager>()).Returns(services);

            var manager = new RoleServiceManager(factory);
            manager.OnStart();
            manager.Run();

            service.Received().Start();
            factory.Received().Tasks(Arg.Any<RoleServiceManager>());
        }

        [TestMethod]
        public void RunThrows()
        {
            var services = new List<IRunnable>();
            var service = Substitute.For<IRunnable>();
            service.When(s => s.Start()).Do(x => { throw new Exception(); });

            services.Add(service);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleServiceManager>()).Returns(services);

            var manager = new RoleServiceManager(factory);
            manager.OnStart();
            manager.Run();

            service.Received().Start();
            factory.Received().Tasks(Arg.Any<RoleServiceManager>());
        }

        [TestMethod]
        public void RunNoServices()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var manager = new RoleServiceManager(factory);
            manager.Run();
        }

        [TestMethod]
        public void OnStart()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var manager = new RoleServiceManager(factory);
            manager.OnStart();
        }

        [TestMethod]
        public void OnStartNoServices()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleServiceManager>()).Returns((IEnumerable<IRunnable>)null);
            
            var manager = new RoleServiceManager(factory);
            manager.OnStart();

            factory.Received().Tasks(Arg.Any<RoleServiceManager>());
        }

        [TestMethod]
        public void OnStop()
        {
            var services = new List<IRunnable>();
            var service = Substitute.For<IRunnable>();
            service.Stop();

            services.Add(service);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleServiceManager>()).Returns(services);

            var manager = new RoleServiceManager(factory);
            manager.OnStart();
            manager.OnStop();

            service.Received().Stop();
            factory.Received().Tasks(Arg.Any<RoleServiceManager>());
        }

        [TestMethod]
        public void OnStopThrows()
        {
            var services = new List<IRunnable>();
            var service = Substitute.For<IRunnable>();
            service.When(s => s.Stop()).Do(x => { throw new Exception(); });

            services.Add(service);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleServiceManager>()).Returns(services);

            var manager = new RoleServiceManager(factory);
            manager.OnStart();
            manager.OnStop();

            service.Received().Stop();
            factory.Received().Tasks(Arg.Any<RoleServiceManager>());
        }

        [TestMethod]
        public void OnStopNoServices()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var manager = new RoleServiceManager(factory);
            manager.OnStop();
        }
    }
}
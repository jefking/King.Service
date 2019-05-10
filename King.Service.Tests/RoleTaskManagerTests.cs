namespace King.Service.Tests
{
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    [TestFixture]
    public class RoleTaskManagerTests
    {
        [Test]
        public void Constructor()
        {
            new RoleTaskManager<object>();
        }

        [Test]
        public void ConstructorFactory()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            new RoleTaskManager<object>(factory);
        }

        [Test]
        public void ConstructorFactoriesParams()
        {
            var factoryA = Substitute.For<ITaskFactory<object>>();
            var factoryB = Substitute.For<ITaskFactory<object>>();
            new RoleTaskManager<object>(factoryA, factoryB);
        }

        [Test]
        public void ConstructorFactoriesEnumerable()
        {
            var r = new Random();
            var max = r.Next(1, 100);
            var factories = new List<ITaskFactory<object>>(max);
            for (var i = 0; i < max; i++)
            {
                factories.Add(Substitute.For<ITaskFactory<object>>());
            }

            new RoleTaskManager<object>(factories);
        }

        [Test]
        public void ConstructorServiceFactoryNull()
        {
            Assert.That(() => new RoleTaskManager<object>((ITaskFactory<object>)null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorAllFactoriesNull()
        {
            var random = new Random();
            var c = random.Next(2, 20);
            var factories = new List<ITaskFactory<object>>();

            for (var i = 0; i < c; i++)
            {
                factories.Add(null);
            }

            Assert.That(() => new RoleTaskManager<object>(factories), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorServiceFactoriesNull()
        {
            Assert.That(() => new RoleTaskManager<object>((IEnumerable<ITaskFactory<object>>)null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorStarterNull()
        {
            var r = new Random();
            var max = r.Next(1, 100);
            var factories = new List<ITaskFactory<object>>(max);
            for (var i = 0; i < max; i++)
            {
                factories.Add(Substitute.For<ITaskFactory<object>>());
            }

            new RoleTaskManager<object>(factories);
            Assert.That(() => new RoleTaskManager<object>(factories, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void IsIRoleServiceManager()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            Assert.IsNotNull(new RoleTaskManager<object>(factory) as RoleTaskManager<object>);
        }

        [Test]
        public void TasksOnInit()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var rtm = new RoleTaskManager<object>(factory);
            Assert.IsNull(rtm.Tasks);
        }

        [Test]
        public void Factories()
        {
            var tasks = new List<IRunnable>();
            var task = Substitute.For<IRunnable>();
            tasks.Add(task);

            var factory1 = Substitute.For<ITaskFactory<object>>();
            factory1.Tasks(Arg.Any<RoleTaskManager<object>>()).Returns(tasks);
            var factory2 = Substitute.For<ITaskFactory<object>>();
            factory2.Tasks(Arg.Any<RoleTaskManager<object>>()).Returns(tasks);

            var manager = new RoleTaskManager<object>(new[] { factory1, factory2 });
            manager.OnStart();
            manager.Run();

            Assert.AreEqual(2, manager.Tasks.Count);
        }

        [Test]
        public void Tasks()
        {
            var tasks = new List<IRunnable>();
            var task = Substitute.For<IRunnable>();
            task.Start();

            tasks.Add(task);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleTaskManager<object>>()).Returns(tasks);

            var manager = new RoleTaskManager<object>(factory);
            manager.OnStart();
            manager.Run();

            Assert.AreEqual(tasks, manager.Tasks);
        }

        [Test]
        public void Run()
        {
            var tasks = new List<IRunnable>();
            var task = Substitute.For<IRunnable>();
            task.Start();

            tasks.Add(task);

            var starter = Substitute.For<IStarter>();

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleTaskManager<object>>()).Returns(tasks);

            var manager = new RoleTaskManager<object>(new []{factory}, starter);
            manager.OnStart();
            manager.Run();

            task.Received().Start();
            factory.Received().Tasks(Arg.Any<RoleTaskManager<object>>());

            starter.Received().Start(Arg.Any<IReadOnlyCollection<IRunnable>>());
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
            var result = manager.OnStart();
            Assert.IsTrue(result);
        }

        [Test]
        public void OnStartTasksNull()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<object>()).Returns((IEnumerable<IRunnable>)null);

            var manager = new RoleTaskManager<object>(factory);
            var result = manager.OnStart();
            Assert.IsTrue(result);
        }

        [Test]
        public void OnStartTwice()
        {
            var tasks = new List<IRunnable>();
            var task = Substitute.For<IRunnable>();
            tasks.Add(task);
            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<object>()).Returns(tasks);

            var manager = new RoleTaskManager<object>(factory);
            var result = manager.OnStart();
            Assert.IsTrue(result);
            result = manager.OnStart();
            Assert.IsTrue(result);
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
            var tasks = new List<IRunnable>();
            var task = Substitute.For<IRunnable>();
            task.Stop();

            tasks.Add(task);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleTaskManager<object>>()).Returns(tasks);

            var manager = new RoleTaskManager<object>(factory);
            manager.OnStart();
            manager.OnStop();

            task.Received().Stop();
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

        [Test]
        public void Dispose()
        {
            var tasks = new List<IRunnable>();
            var task = Substitute.For<IRunnable>();
            task.Start();

            tasks.Add(task);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleTaskManager<object>>()).Returns(tasks);

            using (var manager = new RoleTaskManager<object>(factory))
            {
                manager.OnStart();
                manager.Run();
            }

            task.Received().Start();
            task.Received().Dispose();
            factory.Received().Tasks(Arg.Any<RoleTaskManager<object>>());
        }

        [Test]
        public void DisposeThrows()
        {
            var tasks = new List<IRunnable>();
            var task = Substitute.For<IRunnable>();
            task.When(s => s.Dispose()).Do(x => { throw new Exception(); });

            tasks.Add(task);

            var factory = Substitute.For<ITaskFactory<object>>();
            factory.Tasks(Arg.Any<RoleTaskManager<object>>()).Returns(tasks);

            using (var manager = new RoleTaskManager<object>(factory))
            {
                manager.OnStart();
            }

            task.Received().Dispose();
            factory.Received().Tasks(Arg.Any<RoleTaskManager<object>>());
        }

        [Test]
        public void MaintainOrder()
        {
            var factory = new TaskOrderFactory();
            var manager = new RoleTaskManager<object>(factory);
            manager.OnStart();

            var tasks = manager.Tasks as ReadOnlyCollection<IRunnable>;
            var init = tasks[0];
            Assert.IsNotNull(init as InitializeTask);
            var rr = tasks[1];
            Assert.IsNotNull(rr as RecurringRunner);
        }
        private class TaskOrderFactory : ITaskFactory<object>
        {
            public IEnumerable<IRunnable> Tasks(object config)
            {
                yield return new InitializeTask();

                var runs = Substitute.For<IRuns>();
                runs.MinimumPeriodInSeconds.Returns(1);
                
                yield return new RecurringRunner(runs);
            }
        }
    }
}
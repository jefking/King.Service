namespace King.Service.Tests.Scalability
{
    using King.Service;
    using King.Service.Scalability;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Concurrent;

    [TestFixture]
    public class ScalerTests
    {
        [Test]
        public void Constructor()
        {
            new Scaler<object>();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorUnitsNull()
        {
            new Scaler<object>(null);
        }

        [Test]
        public void IsIScaler()
        {
            Assert.IsNotNull(new Scaler<object>() as IScaler<object>);
        }

        [Test]
        public void Dispose()
        {
            using (new Scaler<object>())
            {
            }
        }

        [Test]
        public void DisposeMany()
        {
            var factory = Substitute.For<ITaskFactory<object>>();

            using (var s = new Scaler<object>())
            {
                s.ScaleUp(factory, new object(), Guid.NewGuid().ToString());
                s.ScaleUp(factory, new object(), Guid.NewGuid().ToString());
                s.ScaleUp(factory, new object(), Guid.NewGuid().ToString());
            }
        }

        [Test]
        public void ScaleUp()
        {
            var factory = Substitute.For<ITaskFactory<object>>();

            var s = new Scaler<object>();
            s.ScaleUp(factory, new object(), Guid.NewGuid().ToString());

            Assert.AreEqual(1, s.CurrentUnits);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScaleUpFactoryNull()
        {
            var s = new Scaler<object>();
            s.ScaleUp(null, new object(), Guid.NewGuid().ToString());

            Assert.AreEqual(1, s.CurrentUnits);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ScaleUpServiceNameNull()
        {
            var factory = Substitute.For<ITaskFactory<object>>();

            var s = new Scaler<object>();
            s.ScaleUp(factory, new object(), null);
        }

        [Test]
        public void ScaleDown()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var task = Substitute.For<IRoleTaskManager<object>>();
            task.OnStop();
            task.Dispose();
            
            var s = new Scaler<object>();
            s.ScaleUp(factory, new object(), Guid.NewGuid().ToString());
            s.ScaleDown(Guid.NewGuid().ToString());

            Assert.AreEqual(0, s.CurrentUnits);

            task.Received().OnStop();
            task.Received().Dispose();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ScaleDownServiceNameNull()
        {
            var s = new Scaler<object>();
            s.ScaleDown(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void InitializeMinimumZero()
        {
            var factory = Substitute.For<ITaskFactory<object>>();

            var s = new Scaler<object>();
            s.Initialize(0, factory, new object(), Guid.NewGuid().ToString());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InitializeFactoryNull()
        {
            var s = new Scaler<object>();
            s.Initialize(1, null, new object(), Guid.NewGuid().ToString());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void InitializeServiceNameNull()
        {
            var factory = Substitute.For<ITaskFactory<object>>();

            var s = new Scaler<object>();
            s.Initialize(1, factory, new object(), null);
        }

        [Test]
        public void Initialize()
        {
            var factory = Substitute.For<ITaskFactory<object>>();

            var s = new Scaler<object>();
            s.Initialize(1, factory, new object(), Guid.NewGuid().ToString());

            Assert.AreEqual(1, s.CurrentUnits);
        }

        [Test]
        public void InitializeMulitple()
        {
            var factory = Substitute.For<ITaskFactory<object>>();

            var s = new Scaler<object>();
            s.Initialize(5, factory, new object(), Guid.NewGuid().ToString());

            Assert.AreEqual(5, s.CurrentUnits);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void IsFirstRunMinimumZero()
        {
            var factory = Substitute.For<ITaskFactory<object>>();

            var s = new Scaler<object>();
            s.IsFirstRun(0);
        }

        [Test]
        public void IsFirstRun()
        {
            var s = new Scaler<object>();
            var result = s.IsFirstRun(1);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsNotFirstRun()
        {
            var factory = Substitute.For<ITaskFactory<object>>();

            var s = new Scaler<object>();
            s.ScaleUp(factory, new object(), Guid.NewGuid().ToString());

            var result = s.IsFirstRun(1);

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldScale()
        {
            var s = new Scaler<object>();
            var result = s.ShouldScale();
            Assert.AreEqual(Direction.None, result);
        }

        [Test]
        public void ShouldScaleUp()
        {
            var task = Substitute.For<IScalable>();
            task.Scale.Returns(true);
            var manager = Substitute.For<IRoleTaskManager<object>>();
            manager.Tasks.Returns(new[] { task });
            var stack = new ConcurrentStack<IRoleTaskManager<object>>();
            stack.Push(manager);

            var s = new Scaler<object>(stack);
            var result = s.ShouldScale();
            Assert.AreEqual(Direction.Up, result);
        }

        [Test]
        public void ShouldScaleDown()
        {
            var task = Substitute.For<IScalable>();
            task.Scale.Returns(false);
            var manager = Substitute.For<IRoleTaskManager<object>>();
            manager.Tasks.Returns(new[] { task });
            var stack = new ConcurrentStack<IRoleTaskManager<object>>();
            stack.Push(manager);

            var s = new Scaler<object>(stack);
            var result = s.ShouldScale();

            Assert.AreEqual(Direction.Down, result);
        }
    }
}
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
        public void IsIScaler()
        {
            Assert.IsNotNull(new Scaler<object>() as IScaler<object>);
        }

        [Test]
        public void ScaleUp()
        {
            var factory = Substitute.For<ITaskFactory<object>>();

            var s = new Scaler<object>();
            s.ScaleUp(factory, Guid.NewGuid().ToString());

            Assert.AreEqual(1, s.CurrentUnits);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScaleUpFactoryNull()
        {
            var units = new ConcurrentStack<IRoleTaskManager<object>>();

            var s = new Scaler<object>();
            s.ScaleUp(null, Guid.NewGuid().ToString());

            Assert.AreEqual(1, units.Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ScaleUpServiceNameNull()
        {
            var factory = Substitute.For<ITaskFactory<object>>();

            var s = new Scaler<object>();
            s.ScaleUp(factory, null);
        }

        [Test]
        public void ScaleDown()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var task = Substitute.For<IRoleTaskManager<object>>();
            task.OnStop();
            task.Dispose();
            
            var s = new Scaler<object>();
            s.ScaleUp(factory, Guid.NewGuid().ToString());
            s.ScaleDown(Guid.NewGuid().ToString());

            Assert.AreEqual(0, s.CurrentUnits);

            task.Received().OnStop();
            task.Received().Dispose();
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ScaleDownServiceNameNull()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var units = new ConcurrentStack<IRoleTaskManager<object>>();

            var s = new Scaler<object>();
            s.ScaleDown(null);
        }
    }
}
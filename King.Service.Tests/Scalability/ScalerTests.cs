namespace King.Azure.BackgroundWorker.Tests.Scalability
{
    using King.Service;
    using King.Service.Scalability;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
            var units = new ConcurrentStack<IRoleTaskManager<object>>();

            var s = new Scaler<object>();
            s.ScaleUp(factory, units, Guid.NewGuid().ToString());

            Assert.AreEqual(1, units.Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScaleUpFactoryNull()
        {
            var units = new ConcurrentStack<IRoleTaskManager<object>>();

            var s = new Scaler<object>();
            s.ScaleUp(null, units, Guid.NewGuid().ToString());

            Assert.AreEqual(1, units.Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScaleUpUnitsNull()
        {
            var factory = Substitute.For<ITaskFactory<object>>();

            var s = new Scaler<object>();
            s.ScaleUp(factory, null, Guid.NewGuid().ToString());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ScaleUpServiceNameNull()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var units = new ConcurrentStack<IRoleTaskManager<object>>();

            var s = new Scaler<object>();
            s.ScaleUp(factory, units, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScaleDownUnitsNull()
        {
            var factory = Substitute.For<ITaskFactory<object>>();

            var s = new Scaler<object>();
            s.ScaleDown(null, Guid.NewGuid().ToString());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ScaleDownServiceNameNull()
        {
            var factory = Substitute.For<ITaskFactory<object>>();
            var units = new ConcurrentStack<IRoleTaskManager<object>>();

            var s = new Scaler<object>();
            s.ScaleDown(units, null);
        }
    }
}
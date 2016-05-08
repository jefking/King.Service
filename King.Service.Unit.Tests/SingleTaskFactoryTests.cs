namespace King.Service.Tests
{
    using King.Service;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    {
        [Test]
        public void Constructor()
        {
            new SingleTaskFactory<object>((t) => { return Substitute.For<IRunnable>(); });
        }

        [Test]
        public void ConstructorCreateNull()
        {
            Assert.That(() => new SingleTaskFactory<object>(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void IsITaskFactory()
        {
            Assert.IsNotNull(new SingleTaskFactory<object>((t) => { return Substitute.For<IRunnable>(); }) as ITaskFactory<object>);
        }

        [Test]
        public void Tasks()
        {
            var task = Substitute.For<IRunnable>();
            var stf = new SingleTaskFactory<object>((t) => { return task; });
            var tasks = stf.Tasks(null);
            Assert.AreEqual(1, tasks.Count());
            Assert.AreEqual(task, tasks.FirstOrDefault());
        }
    }
}

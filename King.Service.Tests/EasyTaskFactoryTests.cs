namespace King.Service.Tests
{
    using King.Azure.Data;
    using King.Service.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class EasyTaskFactoryTests
    {
        class MyFactory : EasyTaskFactory<object>
        {
            public override IEnumerable<IRunnable> Tasks(object passthrough)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void Constructor()
        {
            new MyFactory();
        }

        [Test]
        public void IsITaskFactory()
        {
            Assert.IsNotNull(new MyFactory() as ITaskFactory<object>);
        }

        [Test]
        public void InitializeStorage()
        {
            var storage = Substitute.For<IAzureStorage>();

            var f = new MyFactory();
            var r = f.InitializeStorage(storage);

            Assert.IsNotNull(r as InitializeStorageTask);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InitializeStorageStorageNull()
        {
            var f = new MyFactory();
            var r = f.InitializeStorage(null);
        }

        [Test]
        public void Adaptive()
        {
            var runs = Substitute.For<IDynamicRuns>();

            var f = new MyFactory();
            var r = f.Adaptive(runs);

            Assert.IsNotNull(r as AdaptiveRunner);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AdaptiveRunsNull()
        {
            var f = new MyFactory();
            var r = f.Adaptive(null);
        }

        [Test]
        public void Backoff()
        {
            var runs = Substitute.For<IDynamicRuns>();

            var f = new MyFactory();
            var r = f.Backoff(runs);

            Assert.IsNotNull(r as BackoffRunner);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BackoffRunsNull()
        {
            var f = new MyFactory();
            var r = f.Backoff(null);
        }

        [Test]
        public void Recurring()
        {
            var runs = Substitute.For<IDynamicRuns>();

            var f = new MyFactory();
            var r = f.Recurring(runs);

            Assert.IsNotNull(r as RecurringRunner);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RecurringRunsNull()
        {
            var f = new MyFactory();
            var r = f.Recurring(null);
        }
    }
}
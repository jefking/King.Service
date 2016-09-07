namespace King.Service.Unit.Tests
{
    using King.Service.Timing;
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    public class TaskFinderFactoryTests
    {
        #region Helper
        
        private class ExamplePrivate
        {
            [RunsEvery(100)]
            public void ActionRecurring() { }

            [RunsBetween(1, 100, Strategy.Linear)]
            public bool ActionDynamic() { return true; }

            [Initialize]
            public void InitDataSet() { }
        }
        public struct ExampleStruct
        {
            [RunsEvery(100)]
            public void ActionRecurring() { }

            [RunsBetween(1, 100, Strategy.Linear)]
            public bool ActionDynamic() { return true; }

            [Initialize]
            public void InitDataSet() { }
        }
        #endregion

        [Test]
        public void Constructor()
        {
            new TaskFinderFactory<object>();
        }

        [Test]
        public void IsITaskFactory()
        {
            Assert.IsNotNull(new TaskFinderFactory<object>() as ITaskFactory<object>);
        }

        [Test]
        public void None()
        {
            var tf = new TaskFinderFactory<object>();
            var tasks = tf.Tasks(new object());
            Assert.AreEqual(0, tasks.Count());
        }

        [Test]
        public void Examples()
        {
            var tf = new TaskFinderFactory<ExampleTest>();
            var tasks = tf.Tasks(new ExampleTest());
            Assert.AreEqual(3, tasks.Count());
        }

        [Test]
        public void HappensEveryExample()
        {
            var tf = new TaskFinderFactory<ExampleTest>();
            var tasks = tf.Tasks(new ExampleTest());

            var runners = from t in tasks
                       where t.GetType() == typeof(RecurringRunner)
                       select t;

            Assert.AreEqual(1, runners.Count());
            var every = runners.First();
            Assert.IsNotNull(every);
        }

        [Test]
        public void HappensBetweenExample()
        {
            var tf = new TaskFinderFactory<ExampleTest>();
            var tasks = tf.Tasks(new ExampleTest());

            var runners = from t in tasks
                          where t.GetType() == typeof(AdaptiveRunner)
                          select t;

            Assert.AreEqual(1, runners.Count());
            var every = runners.First();
            Assert.IsNotNull(every);
        }

        [Test]
        public void InitializeExample()
        {
            var tf = new TaskFinderFactory<ExampleTest>();
            var tasks = tf.Tasks(new ExampleTest());

            var runners = from t in tasks
                          where t.GetType() == typeof(InitializeRunner)
                          select t;

            Assert.AreEqual(1, runners.Count());
            var every = runners.First();
            Assert.IsNotNull(every);
        }
    }

    #region Helper
    public class ExampleTest
    {
        [RunsEvery(100)]
        public void ActionRecurring() { }

        [RunsBetween(1, 100, Strategy.Linear)]
        public bool ActionDynamic() { return true; }

        [Initialize]
        public void InitDataSet() { }
    }
    #endregion
}
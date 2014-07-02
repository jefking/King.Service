namespace King.Azure.BackgroundWorker.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class TaskManagerTests
    {
        #region Helper
        public class TestManager : TaskManager
        {
            public TestManager(int dueInSeconds, int periodInSeconds)
                : base(dueInSeconds, periodInSeconds)
            {
            }
            public bool Throw
            {
                get;
                set;
            }
            public override void Run()
            {
                if (this.Throw)
                {
                    throw new InvalidOperationException();
                }
            }
            public void TestDispose()
            {
                base.Dispose(true);
            }
        }
        #endregion

        [TestMethod]
        public void Constructor()
        {
            new TestManager(100, 100);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorDueZero()
        {
            new TestManager(-10, 100);
        }

        [TestMethod]
        public void Dispose()
        {
            using (var m = new TestManager(100, 100))
            {

            }
        }

        [TestMethod]
        public void TestDispose()
        {
            var m = new TestManager(100, 100);
            m.TestDispose();
        }

        [TestMethod]
        public void TestDisposeTimer()
        {
            var m = new TestManager(100, 100);
            m.Start();
            m.TestDispose();
        }

        [TestMethod]
        public void Run()
        {
            var m = new TestManager(100, 100);
            m.Run(this);
        }

        [TestMethod]
        public void RunStateNull()
        {
            var m = new TestManager(100, 100);
            m.Run(null);
        }

        [TestMethod]
        public void RunThrows()
        {
            var m = new TestManager(100, 100)
            {
                Throw = true,
            };
            m.Run(null);
        }

        [TestMethod]
        public void Start()
        {
            var m = new TestManager(100, 100);
            var success = m.Start();
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void Stop()
        {
            var m = new TestManager(100, 100);
            var success = m.Stop();
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void StartStop()
        {
            var m = new TestManager(100, 100);
            var success = m.Start();
            Assert.IsTrue(success);
            success = m.Stop();
            Assert.IsTrue(success);
        }
    }
}
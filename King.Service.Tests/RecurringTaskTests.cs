namespace King.Service.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class RecurringTaskTests
    {
        [Test]
        public void Constructor()
        {
            new RecurringHelper(100, 100);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorDueZero()
        {
            new RecurringHelper(-10, 100);
        }

        [Test]
        public void IsIRunnable()
        {
            Assert.IsNotNull(new RecurringHelper(100, 100) as IRunnable);
        }

        [Test]
        public void IsIDisposable()
        {
            Assert.IsNotNull(new RecurringHelper(100, 100) as IDisposable);
        }

        [Test]
        public void Dispose()
        {
            using (var m = new RecurringHelper(100, 100))
            {
            }
        }

        [Test]
        public void TestDispose()
        {
            var m = new RecurringHelper(100, 100);
            m.TestDispose();
        }

        [Test]
        public void TestDisposeTimer()
        {
            var m = new RecurringHelper(100, 100);
            m.Start();
            m.TestDispose();
        }

        [Test]
        public void StartIn()
        {
            var random = new Random();
            var expected = random.Next();
            var m = new RecurringHelper(expected, 100);
            Assert.AreEqual(expected, m.StartIn.TotalSeconds);
        }

        [Test]
        public void Every()
        {
            var random = new Random();
            var expected = random.Next();
            var m = new RecurringHelper(100, expected);
            Assert.AreEqual(expected, m.Every.TotalSeconds);
        }

        [Test]
        public void Run()
        {
            var m = new RecurringHelper(100, 100);
            m.Run(this);
        }

        [Test]
        public void RunStateNull()
        {
            var m = new RecurringHelper(100, 100);
            m.Run(null);
        }

        [Test]
        public void RunThrows()
        {
            var m = new RecurringHelper(100, 100)
            {
                Throw = true,
            };
            m.Run(null);
        }

        [Test]
        public void Start()
        {
            var m = new RecurringHelper(100, 100);
            var success = m.Start();
            Assert.IsTrue(success);
        }

        [Test]
        public void Stop()
        {
            var m = new RecurringHelper(100, 100);
            var success = m.Stop();
            Assert.IsTrue(success);
        }

        [Test]
        public void StartStop()
        {
            var m = new RecurringHelper(100, 100);
            var success = m.Start();
            Assert.IsTrue(success);
            success = m.Stop();
            Assert.IsTrue(success);
        }

        [Test]
        public void StartZeroStop()
        {
            var m = new RecurringHelper(10, 0);
            var success = m.Start();
            Assert.IsTrue(success);
            success = m.Stop();
            Assert.IsTrue(success);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ChangeTimingZero()
        {
            using (var tm = new RecurringHelper(1000, 100))
            {
                tm.Change(TimeSpan.Zero);
            }
        }

        [Test]
        public void ChangeTimingWithoutStart()
        {
            using (var tm = new RecurringHelper(1000, 100))
            {
                tm.Change(TimeSpan.FromSeconds(100));
            }
        }

        [Test]
        public void ChangeTiming()
        {
            using (var tm = new RecurringHelper(1000, 100))
            {
                tm.Start();
                tm.Change(TimeSpan.FromSeconds(100));
            }
        }
    }
}
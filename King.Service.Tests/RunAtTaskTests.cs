namespace King.Azure.BackgroundWorker.Tests
{
    using King.Service;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class RunAtTaskTests
    {
        #region Class
        public class TestTask : RunAtTask
        {
            public TestTask(byte hour, sbyte minute = -1)
                : base("UseDevelopmentStorage=true;", hour, minute)
            {
            }
            public bool Called = false;
            public override void Run(DateTime currentTime)
            {
                Called = true;
            }
        }
        #endregion

        [Test]
        public void Constructor()
        {
            new TestTask(1, 1);
        }

        [Test]
        public void IsCoordinatedTask()
        {
            Assert.IsNotNull(new TestTask(1, 1) as CoordinatedTask);
        }

        [Test]
        public void HourMax()
        {
            var tt = new TestTask(byte.MaxValue, 1);
            Assert.AreEqual(23, tt.Hour);
        }

        [Test]
        public void HourMin()
        {
            var tt = new TestTask(byte.MinValue, 1);
            Assert.AreEqual(0, tt.Hour);
        }

        [Test]
        public void MinuteMax()
        {
            var tt = new TestTask(1, sbyte.MaxValue);
            Assert.AreEqual(59, tt.Minute);
        }

        [Test]
        public void MinuteMin()
        {
            var tt = new TestTask(1, sbyte.MinValue);
            Assert.AreEqual(-1, tt.Minute);
        }

        [Test]
        public void RunHour()
        {
            var now = DateTime.UtcNow;
            var tt = new TestTask((byte)now.Hour);
            tt.Run();
            Assert.IsTrue(tt.Called);
        }

        [Test]
        public void RunMinute()
        {
            var now = DateTime.UtcNow;
            var tt = new TestTask((byte)now.Hour, (sbyte)now.Minute);
            tt.Run();
            Assert.IsTrue(tt.Called);
        }

        [Test]
        public void RunWrongHour()
        {
            var now = DateTime.UtcNow;
            var hour = now.Hour > 1 ? now.Hour - 1 : 1;
            var tt = new TestTask((byte)hour);
            tt.Run();
            Assert.IsFalse(tt.Called);
        }

        [Test]
        public void RunWrongMinute()
        {
            var now = DateTime.UtcNow;
            var minute = now.Minute > 1 ? now.Minute - 1 : 0;
            var tt = new TestTask((byte)now.Hour, (sbyte)minute);
            tt.Run();
            Assert.IsFalse(tt.Called);
        }
    }
}
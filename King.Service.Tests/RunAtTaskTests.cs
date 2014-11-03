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
            public TestTask(byte hour, sbyte minute)
                : base("UseDevelopmentStorage=true;", hour, minute)
            {
            }
            public override void Run(DateTime currentTime)
            {
                throw new NotImplementedException();
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
    }
}
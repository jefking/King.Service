namespace King.Service.Tests.Scalability
{
    using King.Service.Data;
    using King.Service.Scalability;
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class QueueThroughputTests
    {
        [Test]
        public void Constructor()
        {
            new QueueThroughput();
        }

        [Test]
        public void IsIQueueThroughput()
        {
            Assert.IsNotNull(new QueueThroughput() as IQueueThroughput);
        }

        [TestCase(QueuePriority.Low, (ushort)250)]
        [TestCase(QueuePriority.Medium, (ushort)1400)]
        [TestCase(QueuePriority.High, (ushort)9500)]
        public void MessagesPerScaleUnit(QueuePriority priority, ushort expected)
        {
            var s = new QueueThroughput();
            var data = s.MessagesPerScaleUnit(priority);

            Assert.AreEqual(expected, data);
        }

        [TestCase(QueuePriority.Low, 1)]
        [TestCase(QueuePriority.Medium, 1)]
        [TestCase(QueuePriority.High, 2)]
        public void MinimumScale(QueuePriority priority, byte expected)
        {
            var s = new QueueThroughput();
            var data = s.Scale(priority);

            Assert.AreEqual(expected, data.Minimum);
        }

        [TestCase(QueuePriority.Low, BaseTimes.MinimumStorageTiming)]
        [TestCase(QueuePriority.Medium, 5)]
        [TestCase(QueuePriority.High, 1)]
        public void MinimumFrequency(QueuePriority priority, byte expected)
        {
            var s = new QueueThroughput();
            var data = s.Frequency(priority);

            Assert.AreEqual(expected, data.Minimum);
        }

        [TestCase(QueuePriority.Low, BaseTimes.MaximumStorageTiming)]
        [TestCase(QueuePriority.Medium, 60)]
        [TestCase(QueuePriority.High, BaseTimes.MinimumStorageTiming)]
        public void MaximumFrequency(QueuePriority priority, byte expected)
        {
            var s = new QueueThroughput();
            var data = s.Frequency(priority);

            Assert.AreEqual(expected, data.Maximum);
        }

        [TestCase(QueuePriority.Low, 2)]
        [TestCase(QueuePriority.Medium, 4)]
        [TestCase(QueuePriority.High, 5)]
        public void MaximumScale(QueuePriority priority, byte expected)
        {
            var s = new QueueThroughput();
            var data = s.Scale(priority);

            Assert.AreEqual(expected, data.Maximum);
        }

        [TestCase(QueuePriority.Low, 4)]
        [TestCase(QueuePriority.Medium, 3)]
        [TestCase(QueuePriority.High, 2)]
        public void CheckScaleEvery(QueuePriority priority, byte expected)
        {
            var s = new QueueThroughput();
            var data = s.CheckScaleEvery(priority);

            Assert.AreEqual(expected, data);
        }

        [Test]
        public void RunnerLow()
        {
            var runs = Substitute.For<IDynamicRuns>();
            runs.MinimumPeriodInSeconds.Returns(10);

            var s = new QueueThroughput();
            var scalable = s.Runner(runs, QueuePriority.Low);

            Assert.IsNotNull(runs);
            Assert.IsNotNull(scalable as AdaptiveRunner);
        }

        [Test]
        public void RunnerMedium()
        {
            var runs = Substitute.For<IDynamicRuns>();
            runs.MinimumPeriodInSeconds.Returns(10);

            var s = new QueueThroughput();
            var scalable = s.Runner(runs, QueuePriority.Medium);

            Assert.IsNotNull(runs);
            Assert.IsNotNull(scalable as BackoffRunner);
        }

        [Test]
        public void RunnerHigh()
        {
            var runs = Substitute.For<IDynamicRuns>();
            runs.MinimumPeriodInSeconds.Returns(10);

            var s = new QueueThroughput();
            var scalable = s.Runner(runs, QueuePriority.High);

            Assert.IsNotNull(runs);
            Assert.IsNotNull(scalable as BackoffRunner);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RunnerRunsNull()
        {
            var s = new QueueThroughput();
            s.Runner(null, QueuePriority.High);
        }
    }
}
namespace King.Azure.BackgroundWorker.Tests
{
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BackoffRunnerTests
    {
        [Test]
        public void Constructor()
        {
            var runs = Substitute.For<IBackoffRuns>();
            runs.MinimumPeriodInSeconds.Returns(1);
            runs.MaximumPeriodInSeconds.Returns(100);

            new BackoffRunner(runs);

            var min = runs.Received().MinimumPeriodInSeconds;
            var max = runs.Received().MaximumPeriodInSeconds;
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ConstructorRunsNull()
        {
            new BackoffRunner(null);
        }

        [Test]
        public void Run()
        {
            var runs = Substitute.For<IBackoffRuns>();
            runs.MinimumPeriodInSeconds.Returns(1);
            runs.MaximumPeriodInSeconds.Returns(100);
            runs.Run().Returns(false);

            var task = new BackoffRunner(runs);
            task.Run();

            var min = runs.Received().MinimumPeriodInSeconds;
            var max = runs.Received().MaximumPeriodInSeconds;
            runs.Received().Run();
        }
    }
}

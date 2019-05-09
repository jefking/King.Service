namespace King.Service.Tests
{
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class AdaptiveRunnerTests
    {
        [Test]
        public void Constructor()
        {
            var runs = Substitute.For<IDynamicRuns>();
            runs.MinimumPeriodInSeconds.Returns(1);
            runs.MaximumPeriodInSeconds.Returns(100);

            new AdaptiveRunner(runs);

            var min = runs.Received().MinimumPeriodInSeconds;
            var max = runs.Received().MaximumPeriodInSeconds;
        }

        [Test]
        public void IsAdaptiveTask()
        {
            var runs = Substitute.For<IDynamicRuns>();
            runs.MinimumPeriodInSeconds.Returns(1);
            runs.MaximumPeriodInSeconds.Returns(100);

            Assert.IsNotNull(new AdaptiveRunner(runs) as AdaptiveTask);
        }

        [Test]
        public void ConstructorRunsNull()
        {
            Assert.That(() => new AdaptiveRunner(null), Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void Run()
        {
            var runs = Substitute.For<IDynamicRuns>();
            runs.MinimumPeriodInSeconds.Returns(1);
            runs.MaximumPeriodInSeconds.Returns(100);
            runs.Run().Returns(Task.FromResult(false));

            var task = new AdaptiveRunner(runs);
            task.Run();

            var min = runs.Received().MinimumPeriodInSeconds;
            var max = runs.Received().MaximumPeriodInSeconds;
            runs.Received().Run();
        }
    }
}

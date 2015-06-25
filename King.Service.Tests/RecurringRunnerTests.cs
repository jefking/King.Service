namespace King.Service.Tests
{
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class RecurringRunnerTests
    {
        [Test]
        public void Constructor()
        {
            var runs = Substitute.For<IRuns>();
            runs.MinimumPeriodInSeconds.Returns(100);

            new RecurringRunner(runs);

            var x = runs.Received().MinimumPeriodInSeconds;
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ConstructorRunNull()
        {
            new RecurringRunner(null);
        }

        [Test]
        public void Run()
        {
            var runs = Substitute.For<IRuns>();
            runs.MinimumPeriodInSeconds.Returns(10);
            runs.Run();

            var t = new RecurringRunner(runs);
            t.Run();

            runs.Received().Run();
        }
    }
}
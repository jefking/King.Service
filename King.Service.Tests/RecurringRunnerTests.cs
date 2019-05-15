namespace King.Service.Tests.Unit
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
        public void ConstructorRunNull()
        {
            Assert.That(() => new RecurringRunner(null), Throws.TypeOf<NullReferenceException>());
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
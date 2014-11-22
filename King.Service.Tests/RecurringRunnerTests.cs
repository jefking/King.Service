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
            new RecurringRunner(runs);
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
            runs.Run();

            var t = new RecurringRunner(runs);
            t.Run();

            runs.Received().Run();
        }
    }
}
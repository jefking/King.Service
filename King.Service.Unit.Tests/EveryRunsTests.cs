namespace King.Service.Unit.Tests
{
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class EveryRunsTests
    {
        [Test]
        public void Constructor()
        {
            var method = typeof(object).GetMethod("ToString");
            new EveryRuns(new object(), method);
        }

        [Test]
        public void ConstructorInstanceNull()
        {
            var method = typeof(object).GetMethod("ToString");
            Assert.That(() => new EveryRuns(null, method), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorMethodNull()
        {
            var method = typeof(object).GetMethod("ToString");
            Assert.That(() => new EveryRuns(new object(), null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorFrequencyLow()
        {
            var method = typeof(object).GetMethod("ToString");
            Assert.That(() => new EveryRuns(new object(), method, -1), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsIRuns()
        {
            var method = typeof(object).GetMethod("ToString");
            Assert.IsNotNull(new EveryRuns(new object(), method) as IRuns);
        }

        [Test]
        public void MinimumPeriodInSeconds()
        {
            var random = new Random();
            var f = random.Next(1, 1000);

            var method = typeof(object).GetMethod("ToString");
            var runs = new EveryRuns(new object(), method, f);

            Assert.AreEqual(f, runs.MinimumPeriodInSeconds);
        }

        [Test]
        public async Task Run()
        {
            var instance = Substitute.For<object>();
            instance.ToString().Returns(Guid.NewGuid().ToString());

            var method = instance.GetType().GetMethod("ToString");
            var runs = new EveryRuns(new object(), method);

            var result = await runs.Run();
            Assert.IsTrue(result);

            instance.Received().ToString();
        }
    }
}
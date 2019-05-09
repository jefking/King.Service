namespace King.Service.Unit.Tests
{
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class BetweenRunsTests
    {
        #region Helper
        public class ExampleBetween
        {
            public virtual bool TestMethod()
            {
                return true;
            }
        }
        #endregion

        [Test]
        public void Constructor()
        {
            var method = typeof(object).GetMethod("ToString");
            new BetweenRuns(new object(), method);
        }

        [Test]
        public void ConstructorInstanceNull()
        {
            var method = typeof(object).GetMethod("ToString");
            Assert.That(() => new BetweenRuns(null, method), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorMethodNull()
        {
            var method = typeof(object).GetMethod("ToString");
            Assert.That(() => new BetweenRuns(new object(), null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorFrequencyLow()
        {
            var method = typeof(object).GetMethod("ToString");
            Assert.That(() => new BetweenRuns(new object(), method, -1), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorMinGreaterMax()
        {
            var method = typeof(object).GetMethod("ToString");
            Assert.That(() => new BetweenRuns(new object(), method, 2, 1), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsIDynamicRuns()
        {
            var method = typeof(object).GetMethod("ToString");
            Assert.IsNotNull(new BetweenRuns(new object(), method) as IDynamicRuns);
        }

        [Test]
        public void MinimumPeriodInSeconds()
        {
            var random = new Random();
            var f = random.Next(1, 1000);

            var method = typeof(object).GetMethod("ToString");
            var runs = new BetweenRuns(new object(), method, f, random.Next(1001, 2000));

            Assert.AreEqual(f, runs.MinimumPeriodInSeconds);
        }

        [Test]
        public void MaximumPeriodInSeconds()
        {
            var random = new Random();
            var f = random.Next(2, 1000);

            var method = typeof(object).GetMethod("ToString");
            var runs = new BetweenRuns(new object(), method, 1, f);

            Assert.AreEqual(f, runs.MaximumPeriodInSeconds);
        }

        [Test]
        public async Task RunTrue()
        {
            var instance = Substitute.For<ExampleBetween>();
            instance.TestMethod().Returns(true);

            var method = instance.GetType().GetMethod("TestMethod");
            var runs = new BetweenRuns(instance, method);

            var result = await runs.Run();
            Assert.IsTrue(result);

            instance.Received(1).ToString();
        }

        [Test]
        public async Task RunFalse()
        {
            var instance = Substitute.For<ExampleBetween>();
            instance.TestMethod().Returns(false);

            var method = instance.GetType().GetMethod("TestMethod");
            var runs = new BetweenRuns(instance, method);

            var result = await runs.Run();
            Assert.IsFalse(result);

            instance.Received(1).ToString();
        }
    }
}
namespace King.Service.Unit.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class HappensBetweenAttributeTests
    {
        [Test]
        public void Constructor()
        {
            var random = new Random();
            new HappensBetweenAttribute(random.Next(1, 1000));
        }

        [Test]
        public void ConstructorFrequencyLow()
        {
            Assert.That(() => new HappensBetweenAttribute(-1), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorFrequencyMaxLessMin()
        {
            Assert.That(() => new HappensBetweenAttribute(2, 1), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsAttribute()
        {
            var random = new Random();
            Assert.IsNotNull(new HappensBetweenAttribute(random.Next(1, 1000)) as Attribute);
        }

        [Test]
        public void FrequencyMin()
        {
            var random = new Random();
            var f = random.Next(1, 1000);
            var attr = new HappensBetweenAttribute(f);
            Assert.AreEqual(f, attr.Frequency.Minimum);
        }

        [Test]
        public void FrequencyMax()
        {
            var random = new Random();
            var f = random.Next(2, 1000);
            var attr = new HappensBetweenAttribute(1, f);
            Assert.AreEqual(f, attr.Frequency.Maximum);
        }

        [Test]
        public void Strategy()
        {
            var attr = new HappensBetweenAttribute(1, 2, Timing.Strategy.Linear);
            Assert.AreEqual(Timing.Strategy.Linear, attr.Strategy);
        }
    }
}

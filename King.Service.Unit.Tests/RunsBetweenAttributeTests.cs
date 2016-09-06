namespace King.Service.Unit.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class RunsBetweenAttributeTests
    {
        [Test]
        public void Constructor()
        {
            new RunsBetweenAttribute();
        }

        [Test]
        public void ConstructorFrequencyLow()
        {
            Assert.That(() => new RunsBetweenAttribute(-1), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorFrequencyMaxLessMin()
        {
            Assert.That(() => new RunsBetweenAttribute(2, 1), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsAttribute()
        {
            Assert.IsNotNull(new RunsBetweenAttribute() as Attribute);
        }

        [Test]
        public void FrequencyMin()
        {
            var random = new Random();
            var f = random.Next(1, 1000);
            var attr = new RunsBetweenAttribute(f, random.Next(1001, 2000));
            Assert.AreEqual(f, attr.Frequency.Minimum);
        }

        [Test]
        public void FrequencyMax()
        {
            var random = new Random();
            var f = random.Next(2, 1000);
            var attr = new RunsBetweenAttribute(1, f);
            Assert.AreEqual(f, attr.Frequency.Maximum);
        }

        [Test]
        public void Strategy()
        {
            var attr = new RunsBetweenAttribute(1, 2, Timing.Strategy.Linear);
            Assert.AreEqual(Timing.Strategy.Linear, attr.Strategy);
        }
    }
}

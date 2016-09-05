namespace King.Service.Unit.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class HappensEveryAttributeTests
    {
        [Test]
        public void Constructor()
        {
            var random = new Random();
            new HappensEveryAttribute(random.Next(1, 1000));
        }

        [Test]
        public void ConstructorFrequencyLow()
        {
            Assert.That(() => new HappensEveryAttribute(-1), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsAttribute()
        {
            var random = new Random();
            Assert.IsNotNull(new HappensEveryAttribute(random.Next(1, 1000)) as Attribute);
        }

        [Test]
        public void Frequency()
        {
            var random = new Random();
            var f = random.Next(1, 1000);
            var attr = new HappensEveryAttribute(f);
            Assert.AreEqual(f, attr.Frequency);
        }
    }
}
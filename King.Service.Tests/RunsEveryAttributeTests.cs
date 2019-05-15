namespace King.Service.Tests.Unit
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class RunsEveryAttributeTests
    {
        [Test]
        public void Constructor()
        {
            var random = new Random();
            new RunsEveryAttribute(random.Next(1, 1000));
        }

        [Test]
        public void ConstructorFrequencyLow()
        {
            Assert.That(() => new RunsEveryAttribute(-1), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsAttribute()
        {
            var random = new Random();
            Assert.IsNotNull(new RunsEveryAttribute(random.Next(1, 1000)) as Attribute);
        }

        [Test]
        public void Frequency()
        {
            var random = new Random();
            var f = random.Next(1, 1000);
            var attr = new RunsEveryAttribute(f);
            Assert.AreEqual(f, attr.Frequency);
        }
    }
}
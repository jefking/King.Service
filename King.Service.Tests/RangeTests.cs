namespace King.Service.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class RangeTests
    {
        [Test]
        public void Constructor()
        {
            new Range<object>();
        }

        [Test]
        public void ConstructorValues()
        {
            var random = new Random();
            var min = random.Next();
            var max = random.Next();

            var x = new Range<int>(min, max);

            Assert.AreEqual(min, x.Minimum);
            Assert.AreEqual(max, x.Maximum);
        }

        [Test]
        public void Minimum()
        {
            var random = new Random();
            var expected = random.Next();

            var r = new Range<int>()
            {
                Minimum = expected,
            };

            Assert.AreEqual(expected, r.Minimum);
        }

        [Test]
        public void Maximum()
        {
            var random = new Random();
            var expected = random.Next();

            var r = new Range<int>()
            {
                Maximum = expected,
            };

            Assert.AreEqual(expected, r.Maximum);
        }
    }
}
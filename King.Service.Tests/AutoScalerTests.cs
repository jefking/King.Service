namespace King.Azure.BackgroundWorker.Tests
{
    using King.Service;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class AutoScalerTests
    {
        [Test]
        public void Constructor()
        {
            new AutoScaler();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinimumGreaterThanMaximum()
        {
            new AutoScaler(100, 1);
        }

        [Test]
        public void IsIAutoScaler()
        {
            Assert.IsNotNull(new AutoScaler() as IAutoScaler);
        }

        [Test]
        public void Minimum()
        {
            var scaler = new AutoScaler(1, 100);
            Assert.AreEqual(1, scaler.Minimum);
        }

        [Test]
        public void Maximum()
        {
            var scaler = new AutoScaler(100, 150);
            Assert.AreEqual(150, scaler.Maximum);
        }
    }
}
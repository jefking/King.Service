namespace King.Service.Unit.Tests
{
    using King.Azure.Data;
    using King.Service;
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Tests;
    using System;

    [TestFixture]
    public class RetryProcessorTests
    {
        [Test]
        public void Constructor()
        {
            new RetryProcessor<int>(new ProcHelper());
        }

        [Test]
        public void ConstructorProcessorNull()
        {
            Assert.That(() => new RetryProcessor<int>(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorMaxZero()
        {
            Assert.That(() => new RetryProcessor<int>(new ProcHelper(), 0), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsIProcessor()
        {
            Assert.IsNotNull(new RetryProcessor<int>(new ProcHelper()) as IProcessor<int>);
        }
    }
}
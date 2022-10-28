namespace King.Service.Tests.Unit
{
    using King.Service;
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class SafeProcessorTests
    {
        [Test]
        public void Constructor()
        {
            new SafeProcessor<ProcHelper, int>();
        }

        [Test]
        public void IsIProcessor()
        {
            Assert.IsNotNull(new SafeProcessor<ProcHelper, int>() as IProcessor<int>);
        }

        [Test]
        public void Process()
        {
            var random = new Random();
            var start = random.Next();
            var set = random.Next();
            ProcHelper.Testing = start;
            var p = new SafeProcessor<ProcHelper, int>();
            p.Process(set).Wait();

            Assert.AreEqual(set, ProcHelper.Testing);
        }
    }
}
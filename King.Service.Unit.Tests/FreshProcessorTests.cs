namespace King.Service.Tests
{
    using King.Azure.Data;
    using King.Service;
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using Unit.Tests;

    [TestFixture]
    public class FreshProcessorTests
    {
        [Test]
        public void Constructor()
        {
            new FreshProcessor<ProcHelper, int>();
        }

        [Test]
        public void IsIProcessor()
        {
            Assert.IsNotNull(new FreshProcessor<ProcHelper, int>() as IProcessor<int>);
        }

        [Test]
        public void Process()
        {
            var random = new Random();
            var start = random.Next();
            var set = random.Next();
            ProcHelper.Testing = start;
            var p = new FreshProcessor<ProcHelper, int>();
            p.Process(set);

            Assert.AreEqual(set, ProcHelper.Testing);
        }
    }
}
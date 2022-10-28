namespace King.Service.Tests.Unit
{
    using global::Azure.Data.Wrappers;
    using King.Service;
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

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
        public async Task Process()
        {
            var random = new Random();
            var start = random.Next();
            var set = random.Next();
            ProcHelper.Testing = start;
            var p = new FreshProcessor<ProcHelper, int>();
            await p.Process(set);

            Assert.AreEqual(set, ProcHelper.Testing);
        }
    }
}
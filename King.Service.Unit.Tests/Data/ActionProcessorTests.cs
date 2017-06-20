namespace King.Service.Tests.Data
{
    using global::Azure.Data.Wrappers;
    using King.Service.Data;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class ActionProcessorTests
    {
        [Test]
        public void Constructor()
        {
            new ActionProcessor<object>(async (obj) => { return await Task.FromResult<bool>(true); });
        }

        [Test]
        public void IsIQueueSetup()
        {
            var ap = new ActionProcessor<object>(async (obj) => { return await Task.FromResult<bool>(true); });
            Assert.IsNotNull(ap as IProcessor<object>);
        }

        [Test]
        public void ConstructorActivityNull()
        {
            Assert.That(() => new ActionProcessor<object>(null), Throws.TypeOf<ArgumentNullException>());
        }
        
        [Test]
        public void Process()
        {
            var gotCalled = false;
            var ap = new ActionProcessor<int>(async (i) =>
            {
                gotCalled = true;
                return await Task.FromResult<bool>(true);
            });

            ap.Process(0).Wait();

            Assert.IsTrue(gotCalled);
        }
    }
}
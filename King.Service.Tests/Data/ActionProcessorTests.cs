namespace King.Service.Tests.Data
{
    using System;
    using King.Azure.Data;
    using King.Service.Data;
    using NUnit.Framework;
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
            var random = new Random();
            var id = random.Next();
            var init = id;
            var ap = new ActionProcessor<int>(async (i) =>
            {
                i++;
                return await Task.FromResult<bool>(true);
            });

            ap.Process(id).Wait();

            Assert.AreEqual(init + 1, id);
        }
    }
}
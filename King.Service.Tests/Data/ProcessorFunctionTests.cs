namespace King.Service.Tests.Unit.Data
{
    using System;
    using System.Threading.Tasks;
    using King.Service.Data;
    using NUnit.Framework;

    [TestFixture]
    public class ProcessorFunctionTests
    {
        [Test]
        public void IsIProcessor()
        {
            Assert.IsNotNull(new ProcessorFunction<object>((j) => { return true; }) as IProcessor<object>);
        }

        [Test]
        public void Constructor()
        {
            new ProcessorFunction<object>((j) => { return true; });
        }

        [Test]
        public void ConstructorFunctionNull()
        {
            Assert.That(() => new ProcessorFunction<object>(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task Process()
        {
            var pf = new ProcessorFunction<object>((j) => { return true; });
            var result = await pf.Process(new object());
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ProcessNull()
        {
            var pf = new ProcessorFunction<object>((j) => { return true; });
            var result = await pf.Process(null);
            Assert.IsTrue(result);
        }
    }
}
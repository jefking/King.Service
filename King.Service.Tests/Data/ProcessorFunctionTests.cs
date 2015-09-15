namespace King.Service.Tests.Data
{
    using King.Azure.Data;
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorFunctionNull()
        {
            new ProcessorFunction<object>(null);
        }
    }
}
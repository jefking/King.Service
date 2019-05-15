namespace King.Service.Tests.Unit
{
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class InitializeRunnerTests
    {
        [Test]
        public void Constructor()
        {
            var method = typeof(object).GetMethod("ToString");
            new InitializeRunner(new object(), method);
        }

        [Test]
        public void ConstructorInstanceNull()
        {
            var method = typeof(object).GetMethod("ToString");
            Assert.That(() => new InitializeRunner(null, method), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorMethodNull()
        {
            var method = typeof(object).GetMethod("ToString");
            Assert.That(() => new InitializeRunner(new object(), null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void IsInitializeTask()
        {
            var method = typeof(object).GetMethod("ToString");
            Assert.IsNotNull(new InitializeRunner(new object(), method) as InitializeTask);
        }
        
        [Test]
        public void Run()
        {
            var instance = Substitute.For<object>();

            var method = instance.GetType().GetMethod("ToString");
            var runs = new InitializeRunner(instance, method);

            runs.Run();

            instance.Received(1).ToString();
        }
    }
}
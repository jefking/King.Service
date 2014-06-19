namespace King.Azure.BackgroundWorker.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;

    [TestClass]
    public class RoleServiceManagerTests
    {
        [TestMethod]
        public void Constructor()
        {
            var factory = Substitute.For<IServiceFactory<object>>();
            new RoleServiceManager(factory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorServiceFactoryNull()
        {
            new RoleServiceManager(null);
        }

        [TestMethod]
        public void IsIRoleServiceManager()
        {
            var factory = Substitute.For<IServiceFactory<object>>();
            Assert.IsNotNull(new RoleServiceManager(factory) as IRoleServiceManager);
        }
    }
}

namespace King.Service.Tests.Unit
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class InitializeAttributeTests
    {
        [Test]
        public void Constructor()
        {
            new InitializeAttribute();
        }

        [Test]
        public void IsAttribute()
        {
            Assert.IsNotNull(new InitializeAttribute() as Attribute);
        }
    }
}
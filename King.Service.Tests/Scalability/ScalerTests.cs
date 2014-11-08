namespace King.Azure.BackgroundWorker.Tests.Scalability
{
    using King.Service.Scalability;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class ScalerTests
    {
        [Test]
        public void Constructor()
        {
            new Scaler<object>();
        }
    }
}
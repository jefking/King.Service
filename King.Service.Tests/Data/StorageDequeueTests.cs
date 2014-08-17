namespace King.Service.Tests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using NSubstitute;
    using King.Service.Data;
    using King.Azure.Data;

    [TestFixture]
    public class StorageDequeueTests
    {
        const string ConnectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            var processor = Substitute.For<IProcessor<object>>();
            new StorageDequeue<object>("queue", ConnectionString, processor);
        }
    }
}
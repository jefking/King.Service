namespace King.Service.Tests.Scalability
{
    using System;
    using King.Azure.Data;
    using King.Service.Data;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Scalability;

    [TestFixture]
    public class QueueSimplifiedScalerTests
    {
        [Test]
        public void Constructor()
        {
            var count = Substitute.For<IQueueCount>();
            var creator = Substitute.For<ITaskCreator>();
            new QueueSimplifiedScaler(count, creator);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorCreatorNull()
        {
            var count = Substitute.For<IQueueCount>();
            new QueueSimplifiedScaler(count, null);
        }

        [Test]
        public void IsQueueAutoScaler()
        {
            var count = Substitute.For<IQueueCount>();
            var creator = Substitute.For<ITaskCreator>();
            Assert.IsNotNull(new QueueSimplifiedScaler(count, creator) as QueueAutoScaler<ITaskCreator>);
        }

        [Test]
        public void ScaleUnit()
        {
            var count = Substitute.For<IQueueCount>();
            var creator = Substitute.For<ITaskCreator>();
            Func<IScalable> t = () => {
                return Substitute.For<IScalable>(); ;
            };
            creator.Task.Returns(t);

            Assert.IsNotNull(new QueueSimplifiedScaler(count, creator) as QueueAutoScaler<ITaskCreator>);

            creator.Task().Received();
        }
    }
}
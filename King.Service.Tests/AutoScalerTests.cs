namespace King.Azure.BackgroundWorker.Tests
{
    using King.Service;
    using King.Service.Scalability;
    using King.Service.Timing;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class AutoScalerTests
    {
        private class AutoScalerHelper : AutoScaler<object>
        {

            #region Constructors
            /// <summary>
            /// Default Constructor
            /// </summary>
            public AutoScalerHelper(object configuration = null, byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = 20)
                : base(new Scaler<object>(), configuration, minimum, maximum, checkScaleInMinutes)
            {
            }

            /// <summary>
            /// Mockable Constructor
            /// </summary>
            public AutoScalerHelper(IScaler<object> scaler, object configuration = null, byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = 20)
                : base(scaler, configuration, minimum, maximum, checkScaleInMinutes)
            {
            }
            #endregion

            public override System.Collections.Generic.IEnumerable<IScalable> ScaleUnit(object data)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void Constructor()
        {
            new AutoScalerHelper();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinimumGreaterThanMaximum()
        {
            new AutoScalerHelper(new object(), 100, 1);
        }

        [Test]
        public void IsITaskFactory()
        {
            Assert.IsNotNull(new AutoScalerHelper() as ITaskFactory<object>);
        }

        [Test]
        public void IsRecurringTask()
        {
            Assert.IsNotNull(new AutoScalerHelper() as RecurringTask);
        }

        [Test]
        public void Minimum()
        {
            var scaler = new AutoScalerHelper(new object(), 1, 100);
            Assert.AreEqual(1, scaler.Minimum);
        }

        [Test]
        public void Maximum()
        {
            var scaler = new AutoScalerHelper(new object(), 100, 150);
            Assert.AreEqual(150, scaler.Maximum);
        }
    }
}
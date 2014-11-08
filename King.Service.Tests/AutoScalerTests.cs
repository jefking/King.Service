namespace King.Service.Tests
{
    using King.Service;
    using King.Service.Scalability;
    using King.Service.Timing;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class AutoScalerTests
    {
        #region Helper
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

            public override IEnumerable<IScalable> ScaleUnit(object data)
            {
                yield return new AdaptiveHelper();
            }
        }
        #endregion

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
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMinimumZero()
        {
            new AutoScalerHelper(new object(), 0, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorScalerNull()
        {
            new AutoScalerHelper(null, new object(), 1, 1, 1);
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

        [Test]
        public void MinimumDefault()
        {
            var scaler = new AutoScalerHelper();
            Assert.AreEqual(1, scaler.Minimum);
        }

        [Test]
        public void MaximumDefault()
        {
            var scaler = new AutoScalerHelper();
            Assert.AreEqual(2, scaler.Maximum);
        }

        [Test]
        public void Tasks()
        {
            var scaler = new AutoScalerHelper();
        }

        [Test]
        public void ScaleUnit()
        {
            var scaler = new AutoScalerHelper();
        }
    }
}
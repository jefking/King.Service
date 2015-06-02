namespace King.Service.Scalability
{
    using System;
    using System.Collections.Generic;
    using Data;
    using King.Azure.Data;
    using Timing;

    /// <summary>
    /// Queue Simplified Scaler
    /// </summary>
    public class QueueSimplifiedScaler : QueueAutoScaler<ITaskCreator>
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="count">Count</param>
        /// <param name="creator">Task Creator</param>
        /// <param name="messagesPerScaleUnit">Messages Per-Scale Unit</param>
        /// <param name="minimum">Minimum Scale</param>
        /// <param name="maximum">Maximmum Scale</param>
        /// <param name="checkScaleInMinutes">Check Scale Every</param>
        public QueueSimplifiedScaler(IQueueCount count, ITaskCreator creator, ushort messagesPerScaleUnit = QueueScaler<ITaskCreator>.MessagesPerScaleUnitDefault, byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = BaseTimes.ScaleCheck)
            : base(count, messagesPerScaleUnit, creator, minimum, maximum, checkScaleInMinutes)
        {
            if (null == creator)
            {
                throw new ArgumentNullException("creator");
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Scale Unit
        /// </summary>
        /// <param name="creator">Task Creator</param>
        /// <returns>Task(s)</returns>
        public override IEnumerable<IScalable> ScaleUnit(ITaskCreator creator)
        {
            yield return creator.Task();
        }
        #endregion
    }
}
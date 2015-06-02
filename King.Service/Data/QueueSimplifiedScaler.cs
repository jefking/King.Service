namespace King.Service.Data
{
    using System.Collections.Generic;
    using King.Azure.Data;

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
        /// <param name="task">Task</param>
        /// <param name="messagesPerScaleUnit">Messages Per-Scale Unit</param>
        /// <param name="minimum">Minimum Scale</param>
        /// <param name="maximum">Maximmum Scale</param>
        /// <param name="checkScaleInMinutes">Check Scale Every</param>
        public QueueSimplifiedScaler(IQueueCount count, ITaskCreator task = null, ushort messagesPerScaleUnit = 10, byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = 2)
            : base(count, messagesPerScaleUnit, task, minimum, maximum, checkScaleInMinutes)
        {
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
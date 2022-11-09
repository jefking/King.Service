namespace King.Service.Scalability
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Queue Scaler
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class QueueScaler<T> : Scaler<T>
    {
        #region Members
        /// <summary>
        /// Messages per-Scale Unit Default
        /// </summary>
        public const ushort MessagesPerScaleUnitDefault = 250;

        /// <summary>
        /// Messages per-Scale Unit
        /// </summary>
        protected readonly ushort messagesPerScaleUnit = MessagesPerScaleUnitDefault;

        /// <summary>
        /// Queue Count
        /// </summary>
        protected readonly IQueueCount queueCount = null;
        #endregion
        
        #region Constructors
        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public QueueScaler(IQueueCount queueCount, ushort messagesPerScaleUnit = MessagesPerScaleUnitDefault)
        {
            if (null == queueCount)
            {
                throw new ArgumentNullException("queueCount");
            }

            this.queueCount = queueCount;
            this.messagesPerScaleUnit = messagesPerScaleUnit < 10 ? (ushort)MessagesPerScaleUnitDefault : messagesPerScaleUnit;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Maximum
        /// </summary>
        public virtual ushort Maximum
        {
            get
            {
                return this.messagesPerScaleUnit;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Should Scale
        /// </summary>
        /// <returns>Direction</returns>
        public override bool? ShouldScale()
        {
            try
            {
                var messageCount = queueCount.ApproixmateMessageCount().Result;
                if (messageCount.HasValue)
                {
                    var result = (double)messageCount.Value / messagesPerScaleUnit;
                    return result == this.CurrentUnits ? (bool?)null : result > this.CurrentUnits;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            return null;
        }
        #endregion
    }
}
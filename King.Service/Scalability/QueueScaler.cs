namespace King.Service.Scalability
{
    using King.Azure.Data;
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
        /// Maximum
        /// </summary>
        protected readonly ushort maximum = 100;

        /// <summary>
        /// Queue Count
        /// </summary>
        protected readonly IQueueCount queueCount = null;
        #endregion
        
        #region Constructors
        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="units">Scale Units</param>
        public QueueScaler(IQueueCount queueCount, ushort maximum = 100)
        {
            if (null == queueCount)
            {
                throw new ArgumentNullException("queueCount");
            }

            this.queueCount = queueCount;
            this.maximum = maximum < 10 ? (ushort)100 : maximum;
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
                return this.maximum;
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
                var result = queueCount.ApproixmateMessageCount().Result;
                return result > maximum;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());

                return null;
            }
        }
        #endregion
    }
}
namespace King.Service.Data
{
    using King.Service.Timing;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Dequeue Batch Task for Queues
    /// </summary>
    public class DequeueBatch<T> : Dequeue<T>
    {
        #region Members
        /// <summary>
        /// Batch Count
        /// </summary>
        protected byte batchCount = 5;

        /// <summary>
        /// Minimum Batch Size
        /// </summary>
        public const byte MinimumBatchSize = 1;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="processor">Processor</param>
        /// <param name="poller">Poller</param>
        /// <param name="batchCount">Number of items to dequeue at a time</param>
        /// <param name="minimumPeriodInSeconds">Minimum, time in seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum, time in seconds</param>
        public DequeueBatch(IPoller<T> poller, IProcessor<T> processor, byte batchCount = 5, int minimumPeriodInSeconds = BaseTimes.DefaultMinimumTiming, int maximumPeriodInSeconds = BaseTimes.DefaultMaximumTiming)
            : base(poller, processor, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
            this.batchCount = batchCount == byte.MinValue ? MinimumBatchSize : batchCount;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Batch Count
        /// </summary>
        public virtual int BatchCount
        {
            get
            {
                return this.batchCount;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run
        /// </summary>
        /// <returns>Work was done</returns>
        public override async Task<bool> Run()
        {
            var workDone = false;
            var messages = await this.poller.PollMany(this.BatchCount);
            if (null != messages && messages.Any())
            {
                workDone = true;

                Trace.TraceInformation("{0} messages dequeued for processing by: {1}.", messages.Count(), base.processor.GetType());

                await this.Process(messages);
            }
            else
            {

                Trace.TraceInformation("No messages were dequeued.");
            }

            return workDone;
        }

        /// <summary>
        /// Process Messages
        /// </summary>
        /// <param name="msgs">Messages</param>
        /// <returns>Task</returns>
        protected virtual async Task Process(IEnumerable<IQueued<T>> msgs)
        {
            foreach (var msg in msgs.Where(m => m != null))
            {
                await this.Process(msg);
            }
        }
        #endregion
    }
}
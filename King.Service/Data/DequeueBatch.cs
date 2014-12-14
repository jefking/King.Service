namespace King.Service.Data
{
    using King.Azure.Data;
    using King.Service.Timing;
    using System;
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
        public DequeueBatch(IPoller<T> poller, IProcessor<T> processor, byte batchCount = 5, int minimumPeriodInSeconds = BaseTimes.MinimumStorageTiming, int maximumPeriodInSeconds = BaseTimes.MaximumStorageTiming)
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
            var worked = false;

            var timing = new Stopwatch();
            timing.Start();

            var messages = await this.poller.PollMany(this.BatchCount);
            if (null != messages && messages.Any())
            {
                Trace.TraceInformation("{0} messages dequeued.", messages.Count());

                worked = true;

                foreach (var msg in messages.Where(m => m != null))
                {
                    await this.Process(msg);
                }

                this.RunCompleted(TimeSpan.FromTicks(timing.ElapsedTicks));
            }
            else
            {

                Trace.TraceInformation("No messages were dequeued.");
            }

            timing.Stop();

            return worked;
        }

        /// <summary>
        /// Signal for completion
        /// </summary>
        /// <param name="duration">Duration</param>
        protected virtual void RunCompleted(TimeSpan duration)
        {
        }
        #endregion
    }
}
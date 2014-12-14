namespace King.Service.Data
{
    using King.Azure.Data;
    using King.Service.Timing;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Dequeue Batch Task for Queues
    /// </summary>
    public class DequeueBatchDynamic<T> : DequeueBatch<T>
    {
        #region Members
        /// <summary>
        /// Tracking
        /// </summary>
        protected readonly ITimingTracker tracker = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="processor">Processor</param>
        /// <param name="poller">Poller</param>
        /// <param name="tracker">Tracker</param>
        /// <param name="minimumPeriodInSeconds">Minimum, time in seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum, time in seconds</param>
        public DequeueBatchDynamic(IPoller<T> poller, IProcessor<T> processor, ITimingTracker tracker, int minimumPeriodInSeconds = BaseTimes.MinimumStorageTiming, int maximumPeriodInSeconds = BaseTimes.MaximumStorageTiming)
            : base(poller, processor, DequeueBatch<T>.MinimumBatchSize, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
            if (null == tracker)
            {
                throw new ArgumentNullException("tracker");
            }

            this.tracker = tracker;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run
        /// </summary>
        /// <returns>Work was done</returns>
        public override Task<bool> Run()
        {
            var timing = new Stopwatch();
            timing.Start();

            var result = base.Work().Result;

            this.RunCompleted(result, TimeSpan.FromTicks(timing.ElapsedTicks));

            timing.Stop();

            return Task.FromResult(0 < result);
        }

        /// <summary>
        /// Signal for completion
        /// </summary>
        /// <param name="count">Batch Count</param>
        /// <param name="duration">Duration</param>
        protected virtual void RunCompleted(int count, TimeSpan duration)
        {
            Trace.TraceInformation("Dequeue message processing took: {0}; for {1} messages.", duration, count);

            if (count == this.batchCount)
            {
                var result = this.tracker.Calculate(duration, this.batchCount);

                if (result != this.batchCount)
                {
                    this.batchCount = result;

                    Trace.TraceInformation("Current batch size set to: {0}.", this.batchCount);
                }
            }
        }
        #endregion
    }
}
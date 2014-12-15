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
    /// <typeparam name="T">Type</typeparam>
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
        public override async Task<bool> Run()
        {
            var workDone = false;
            var timing = new Stopwatch();
            timing.Start();

            var messages = await this.poller.PollMany(this.BatchCount);
            if (null != messages && messages.Any())
            {
                workDone = true;

                Trace.TraceInformation("{0} messages dequeued.", messages.Count());

                var tasks = new Task[messages.Count()];
                var i = 0;
                foreach (var msg in messages.Where(m => m != null))
                {
                    tasks[i] = this.Process(msg);
                    i++;
                }

                Task.WaitAll(tasks);

                this.RunCompleted(messages.Count(), TimeSpan.FromTicks(timing.ElapsedTicks));
            }
            else
            {

                Trace.TraceInformation("No messages were dequeued.");
            }

            timing.Stop();

            return workDone;
        }

        /// <summary>
        /// Signal for completion
        /// </summary>
        /// <param name="count">Batch Count</param>
        /// <param name="duration">Duration</param>
        protected virtual void RunCompleted(int count, TimeSpan duration)
        {
            Trace.TraceInformation("Dequeue message processing took: {0}; for {1} messages.", duration, count);

            if (count == this.BatchCount)
            {
                var result = this.tracker.Calculate(duration, this.batchCount);

                if (result != this.BatchCount)
                {
                    this.batchCount = result;

                    Trace.TraceInformation("Current batch size set to: {0}.", this.BatchCount);
                }
            }
        }
        #endregion
    }
}
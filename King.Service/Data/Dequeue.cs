﻿namespace King.Service.Data
{
    using global::Azure.Data.Wrappers;
    using King.Service.Timing;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Dequeue Task for Queues
    /// </summary>
    /// <remarks>
    /// Generic, built for any dequeuing infrastructure
    /// </remarks>
    public class Dequeue<T> : IDynamicRuns
    {
        #region Members
        /// <summary>
        /// Processor
        /// </summary>
        protected readonly IProcessor<T> processor = null;

        /// <summary>
        /// Poller
        /// </summary>
        protected readonly IPoller<T> poller = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="processor">Processor</param>
        /// <param name="poller">Poller</param>
        /// <param name="minimumPeriodInSeconds">Minimum, time in seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum, time in seconds</param>
        public Dequeue(IPoller<T> poller, IProcessor<T> processor, int minimumPeriodInSeconds = BaseTimes.DefaultMinimumTiming, int maximumPeriodInSeconds = BaseTimes.DefaultMaximumTiming)
        {
            if (null == poller)
            {
                throw new ArgumentNullException("poller");
            }
            if (null == processor)
            {
                throw new ArgumentNullException("processor");
            }

            this.poller = poller;
            this.processor = processor;
            this.MinimumPeriodInSeconds = 0 >= minimumPeriodInSeconds ? BaseTimes.DefaultMinimumTiming : minimumPeriodInSeconds;
            this.MaximumPeriodInSeconds = minimumPeriodInSeconds >= maximumPeriodInSeconds ? BaseTimes.DefaultMaximumTiming : maximumPeriodInSeconds;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run
        /// </summary>
        /// <returns>Work was done</returns>
        public virtual async Task<bool> Run()
        {
            var worked = false;

            var message = await this.poller.Poll();
            if (null != message)
            {
                worked = true;

                await this.Process(message);
            }
            else
            {
                Trace.TraceInformation("No message was dequeued.");
            }

            return worked;
        }

        /// <summary>
        /// Process Message
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        protected virtual async Task Process(IQueued<T> message)
        {
            try
            {
                var data = await message.Data();
                if (null != data)
                {
                    var successful = await this.processor.Process(data);
                    if (successful)
                    {
                        await message.Complete();
                    }
                    else
                    {
                        await message.Abandon();
                    }
                }
                else
                {
                    await message.Abandon();
                }
            }
            catch
            {
                message.Abandon().Wait();

                throw;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Minimum Period In Seconds
        /// </summary>
        public virtual int MinimumPeriodInSeconds
        {
            get;
            private set;
        }

        /// <summary>
        /// Maximum Period In Seconds
        /// </summary>
        public virtual int MaximumPeriodInSeconds
        {
            get;
            private set;
        }
        #endregion
    }
}
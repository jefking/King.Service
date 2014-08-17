namespace King.Service.Data
{
    using King.Azure.Data;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Dequeue Task for Queues
    /// </summary>
    /// <remarks>
    /// Generic, built for any dequeuing infrastructure
    /// </remarks>
    public class Dequeue<T> : IBackoffRuns
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
        public Dequeue(IPoller<T> poller, IProcessor<T> processor, int minimumPeriodInSeconds = 15, int maximumPeriodInSeconds = 300)
        {
            if (null == poller)
            {
                throw new ArgumentNullException("poller");
            }
            if (null == processor)
            {
                throw new ArgumentNullException("processor");
            }
            if (0 >= minimumPeriodInSeconds)
            {
                throw new ArgumentException("Minimum Period In Seconds must be greater than 0.");
            }
            if (minimumPeriodInSeconds >= maximumPeriodInSeconds)
            {
                throw new ArgumentException("Mminimum Period In Seconds must be less than Maximum Period In Seconds");
            }

            this.poller = poller;
            this.processor = processor;
            this.MinimumPeriodInSeconds = minimumPeriodInSeconds;
            this.MaximumPeriodInSeconds = maximumPeriodInSeconds;
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
                    var task = message.Abandon();
                    task.Wait();

                    throw;
                }
            }
            else
            {
                Trace.TraceInformation("No message was dequeued.");
            }

            return worked;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Minimum Period In Seconds
        /// </summary>
        public int MinimumPeriodInSeconds
        {
            get;
            private set;
        }

        /// <summary>
        /// Maximum Period In Seconds
        /// </summary>
        public int MaximumPeriodInSeconds
        {
            get;
            private set;
        }
        #endregion
    }
}
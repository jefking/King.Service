namespace King.Azure.BackgroundWorker.Data
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Dequeue Task for Queues
    /// </summary>
    public class Dequeue<T> : IBackoffRuns
    {
        #region Members
        /// <summary>
        /// Dequeue Processor
        /// </summary>
        private readonly IDequeueProcessor<T> processor = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="processor">Processor</param>
        /// <param name="minimumPeriodInSeconds">Minimum, time in seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum, time in seconds</param>
        public Dequeue(IDequeueProcessor<T> processor, int minimumPeriodInSeconds = 15, int maximumPeriodInSeconds = 300)
        {
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

            this.processor = processor;
            this.MinimumPeriodInSeconds = minimumPeriodInSeconds;
            this.MaximumPeriodInSeconds = maximumPeriodInSeconds;
        }
        #endregion

        #region Methods
        public async Task<bool> Run()
        {
            var worked = false;
            
            var message = await this.processor.Poll();
            if (null != message)
            {
                worked = true;

                var data = message.Data;

                if (null != data)
                {
                    var successful = await this.processor.Process(data);
                    if (successful)
                    {
                        await message.Delete();
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

            return worked;
        }
        #endregion

        #region Properties
        public int MinimumPeriodInSeconds
        {
            get;
            private set;
        }

        public int MaximumPeriodInSeconds
        {
            get;
            private set;
        }
        #endregion
    }
}
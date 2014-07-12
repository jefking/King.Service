namespace King.Azure.BackgroundWorker.Data
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Dequeue Task for Queues
    /// </summary>
    public class Dequeue<T> : IBackoffRuns
    {
        private readonly IDequeueProcessor<T> poller;

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="minimumPeriodInSeconds">Minimum, time in seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum, time in seconds</param>
        public Dequeue(int minimumPeriodInSeconds = 15, int maximumPeriodInSeconds = 300)
        {
            if (0 >= minimumPeriodInSeconds)
            {
                throw new ArgumentException("Minimum Period In Seconds must be greater than 0.");
            }
            if (minimumPeriodInSeconds >= maximumPeriodInSeconds)
            {
                throw new ArgumentException("Mminimum Period In Seconds must be less than Maximum Period In Seconds");
            }

            this.MinimumPeriodInSeconds = minimumPeriodInSeconds;
            this.MaximumPeriodInSeconds = maximumPeriodInSeconds;
        }
        #endregion

        #region Methods
        public bool Run()
        {
            var worked = false;
            var task = poller.Poll();
            task.Wait();
            
            var data = task.Result;
            if (null != data)
            {
                worked = true;

                var converted = JsonConvert.DeserializeObject<T>(data.AsString);

                if (null != converted)
                {
                    var processing = this.poller.Process(converted);
                    processing.Wait();

                    var successful = processing.Result;
                    if (successful)
                    {
                        // Delete Message
                    }
                    else
                    {
                        // Bad mojo
                    }
                }
                else
                {
                    // Bad mojo.
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
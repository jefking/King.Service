namespace King.Azure.BackgroundWorker.Data
{
    using System;

    /// <summary>
    /// Dequeue Task for Queues
    /// </summary>
    public class Dequeue : IBackoffRuns
    {
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
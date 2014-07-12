namespace King.Azure.BackgroundWorker.Data
{
    using System;

    /// <summary>
    /// Dequeue Task for Queues
    /// </summary>
    public class Dequeue : IBackoffRuns
    {
        public bool Run()
        {
            return false;
        }

        public int MinimumPeriodInSeconds
        {
            get
            {
                return 1;
            }
        }

        public int MaximumPeriodInSeconds
        {
            get
            {
                return 600;
            }
        }
    }
}
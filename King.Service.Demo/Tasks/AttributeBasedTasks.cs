namespace King.Service.Demo.Tasks
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Simple implementation for regularly used tasks
    /// </summary>
    public class AttributeBasedTasks
    {
        /// <summary>
        /// Occurs once during start-up
        /// </summary>
        [Initialize]
        public void InitializeDataStore()
        {
            Trace.TraceInformation("Initialize.");
        }

        /// <summary>
        /// Runs on a set frequency
        /// </summary>
        [RunsEvery]
        public void Recurring()
        {
            Trace.TraceInformation("Working.");
        }

        /// <summary>
        /// Runs between certain frequency
        /// </summary>
        [RunsBetween]
        public bool Dynamic()
        {
            var random = new Random();
            var workWasDone = (random.Next() % 2) == 0;

            Trace.TraceInformation("Work was done: {0}", workWasDone);

            return workWasDone;
        }

        /// <summary>
        /// Occurs once during startup
        /// Runs on a set frequency
        /// </summary>
        [RunsEvery]
        [Initialize]
        public void WhyWouldYouDoThis()
        {
            Trace.TraceInformation("I don't know.");
        }
    }
}

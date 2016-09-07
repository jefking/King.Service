namespace King.Service.CloudService.Role
{
    using System;
    using System.Diagnostics;

    public class AttributeBasedTasks
    {
        [Initialize]
        public void InitializeDataStore()
        {
            Trace.TraceInformation("Initialize.");
        }

        [RunsEvery]
        public void Recurring()
        {
            Trace.TraceInformation("Working.");
        }

        [RunsBetween]
        public bool Dynamic()
        {
            var random = new Random();
            var workWasDone = (random.Next() % 2) == 0;

            Trace.TraceInformation("Work was done: {0}", workWasDone);

            return workWasDone;
        }

        [RunsEvery]
        [Initialize]
        public void WhyWouldYouDoThis()
        {
            Trace.TraceInformation("I don't know.");
        }
    }
}
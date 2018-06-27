namespace King.Service.Demo.Tasks
{
    using King.Service;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Task that slows down, or speeds up dependant on load
    /// </summary>
    public class Backoff : BackoffTask
    {
        public override void Run(out bool workWasDone)
        {
            var random = new Random();
            workWasDone = (random.Next() % 2) == 0;

            Trace.TraceInformation("Work was done: {0}", workWasDone);
        }
    }
}
namespace King.Service.CloudService.Role
{
    using System;
    using System.Diagnostics;
    using King.Service;

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
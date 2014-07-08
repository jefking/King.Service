namespace Worker
{
    using King.Azure.BackgroundWorker;
    using System;
    using System.Diagnostics;

    public class Backoff : BackoffTask
    {
        public Backoff()
            :base(1, 6000)
        {
        }

        public override void Run(out bool workWasDone)
        {
            workWasDone = false;

            Trace.TraceInformation("Work was not done: {0}", DateTime.UtcNow);
        }
    }
}
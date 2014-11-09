namespace Worker
{
    using King.Service;
    using System;
    using System.Diagnostics;

    public class Backoff : BackoffTask
    {
        public Backoff()
            : base(1, 120) { }
        public override void Run(out bool workWasDone)
        {
            Trace.TraceInformation("B:{0}", System.DateTime.UtcNow.TimeOfDay);

            var random = new Random();
            workWasDone = base.Every.TotalSeconds == 120;

            Trace.TraceInformation("Work was done: {0}", workWasDone);
        }
    }
}
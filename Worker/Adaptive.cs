namespace Worker
{
    using King.Service;
    using System;
    using System.Diagnostics;

    public class Adaptive : AdaptiveTask
    {
        public Adaptive()
            : base(1, 120) { }
        public override void Run(out bool workWasDone)
        {
            Trace.TraceInformation("A:{0}", System.DateTime.UtcNow.TimeOfDay);

            var random = new Random();
            workWasDone = (random.Next() % 2) == 0;

            Trace.TraceInformation("Work was done: {0}", workWasDone);
        }
    }
}
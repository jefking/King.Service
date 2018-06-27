namespace King.Service.Demo.Tasks
{
    using King.Service;
    using System;
    using System.Diagnostics;

    public class Adaptive : AdaptiveTask
    {
        public override void Run(out bool workWasDone)
        {
            var random = new Random();
            workWasDone = (random.Next() % 2) == 0;

            Trace.TraceInformation("Work was done: {0}", workWasDone);
        }
    }
}
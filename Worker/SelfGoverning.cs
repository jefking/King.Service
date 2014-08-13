namespace Worker
{
    using King.Service;
    using System;
    using System.Diagnostics;

    public class SelfGoverning : SelfGoverningTask
    {
        public override void Run(out bool workWasDone)
        {
            workWasDone = (DateTime.Now.Ticks % 2) == 0;

            Trace.TraceInformation("Work was done: {0}", workWasDone);
        }
    }
}
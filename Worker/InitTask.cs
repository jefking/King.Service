﻿namespace Worker
{
    using King.Service;
    using System.Diagnostics;

    public class InitTask : InitializeTask
    {
        public override void Run()
        {
            Trace.TraceInformation("Initialize a resource here.");
        }
    }
}
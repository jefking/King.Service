namespace Worker
{
    using King.Azure.BackgroundWorker;
    using System.Diagnostics;

    public class InitTask : InitializeTask
    {
        public override void Run()
        {
            Trace.TraceInformation("Initialize a resource here.");
        }
    }
}
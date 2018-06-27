namespace King.Service.Demo.Tasks
{
    using King.Service;
    using System.Diagnostics;

    public class InitTask : InitializeTask
    {
        public override void Run()
        {
            Trace.TraceInformation("initializing");
        }
    }
}
namespace King.Service.CloudService.Role
{
    using System.Diagnostics;
    using King.Service;

    public class InitTask : InitializeTask
    {
        public override void Run()
        {
            Trace.TraceInformation("initializing");
        }
    }
}
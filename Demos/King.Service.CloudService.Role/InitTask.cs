namespace King.Service.CloudService.Role
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
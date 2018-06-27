namespace King.Service.Demo.Tasks
{
    using King.Service;
    using System.Diagnostics;

    /// <summary>
    /// Task occurs onces at start-up
    /// </summary>
    public class InitTask : InitializeTask
    {
        public override void Run()
        {
            Trace.TraceInformation("initializing");
        }
    }
}
namespace Worker
{
    using King.Service;
    using System.Diagnostics;

    public class Task : TaskManager
    {
        public override void Run()
        {
            Trace.TraceInformation("Task that you want to do");
        }
    }
}
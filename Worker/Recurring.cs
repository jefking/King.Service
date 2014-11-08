namespace Worker
{
    using King.Service;
    using System.Diagnostics;

    public class Recurring : RecurringTask
    {
        public override void Run()
        {
            Trace.TraceInformation("Task that you want to do");
        }
    }
}
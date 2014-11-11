namespace Worker
{
    using King.Service;
    using System.Diagnostics;

    public class Recurring : RecurringTask
    {
        public override void Run()
        {
            Trace.TraceInformation("working");
        }
    }
}
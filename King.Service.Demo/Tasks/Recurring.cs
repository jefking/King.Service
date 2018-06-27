namespace King.Service.Demo.Tasks
{
    using King.Service;
    using System.Diagnostics;

    /// <summary>
    /// Recurring Task
    /// </summary>
    public class Recurring : RecurringTask
    {
        public override void Run()
        {
            Trace.TraceInformation("working");
        }
    }
}
namespace Worker
{
    using King.Azure.BackgroundWorker;
    using System.Diagnostics;

    public class Task : Manager
    {
        public Task()
            : base(10, 60)
        {
        }

        public override void Run()
        {
            Trace.TraceInformation("Task that you want to do");
        }
    }
}
namespace Worker
{
    using King.Azure.BackgroundWorker;
    using System.Diagnostics;

    public class Coordinated : CoordinatedTask
    {
        public Coordinated()
            : base("UseDevelopmentStorage=true", 10)
        {
        }

        public override void Run()
        {
            Trace.TraceInformation("Coordinated Task that you want to do");
        }
    }
}

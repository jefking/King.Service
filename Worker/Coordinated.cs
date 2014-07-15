namespace Worker
{
    using King.Azure.BackgroundWorker;
    using System.Diagnostics;

    public class Coordinated : CoordinatedTask
    {
        public Coordinated(string connectionString)
            : base(connectionString)
        {
        }

        public override void Run()
        {
            Trace.TraceInformation("Coordinated Task that you want to do");
        }
    }
}

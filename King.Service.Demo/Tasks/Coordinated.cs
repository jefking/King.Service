namespace King.Service.Demo.Tasks
{
    using King.Service;
    using System.Diagnostics;

    /// <summary>
    /// Occurs in regular interval across compute nodes
    /// </summary>
    public class Coordinated : CoordinatedTask
    {
        public Coordinated(string connectionString)
            : base(connectionString)
        {
        }

        public override void Run()
        {
            Trace.TraceInformation("coordinated");
        }
    }
}
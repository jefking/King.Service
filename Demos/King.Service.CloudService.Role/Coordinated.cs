namespace King.Service.CloudService.Role
{
    using King.Service;
    using System.Diagnostics;

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
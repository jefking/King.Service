namespace King.Service.Demo.Tasks
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Task that occurs Once Daily, across compute nodes
    /// </summary>
    public class OnceDaily : RunAtTask
    {
        public OnceDaily(string connectionString)
            : base(connectionString, (byte) DateTime.UtcNow.Hour)
        {
        }

        public override void Run(DateTime currentTime)
        {
            Trace.TraceInformation("{0} run @ {1}", base.Name, currentTime);
        }
    }
}
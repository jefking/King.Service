namespace Worker
{
    using King.Azure.BackgroundWorker;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using System.Diagnostics;
    using System.Net;
    using System.Threading;

    public class WorkerRole : RoleEntryPoint
    {
        private readonly RoleServiceManager manager = new RoleServiceManager(new Factory());

        public override void Run()
        {
            this.manager.Run();

            base.Run();
        }

        public override bool OnStart()
        {
            return this.manager.OnStart();
        }

        public override void OnStop()
        {
            this.manager.OnStop();

            base.OnStop();
        }
    }
}
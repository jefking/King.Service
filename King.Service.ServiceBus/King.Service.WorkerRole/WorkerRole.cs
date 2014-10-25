namespace King.Service.WorkerRole
{
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class WorkerRole : RoleEntryPoint
    {
        private RoleTaskManager manager = new RoleTaskManager(new King.Service.ServiceBus.Factory());

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

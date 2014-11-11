namespace King.Service.WorkerRole
{
    using King.Service.ServiceBus;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class WorkerRole : RoleEntryPoint
    {
        private readonly IRoleTaskManager<Configuration> manager = new RoleTaskManager<Configuration>(new Factory());

        public override void Run()
        {
            this.manager.Run();

            base.Run();
        }

        public override bool OnStart()
        {
            var config = new Configuration()
            {
                PollingName = "polling",
                EventsName = "events",
            };

            return this.manager.OnStart(config);
        }

        public override void OnStop()
        {
            this.manager.OnStop();

            base.OnStop();
        }
    }
}

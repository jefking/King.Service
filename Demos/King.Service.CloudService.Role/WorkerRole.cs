namespace King.Service.CloudService.Role
{
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class WorkerRole : RoleEntryPoint
    {
        /// <summary>
        /// Role Service Manager
        /// </summary>
        private readonly IRoleTaskManager<Configuration> manager = new RoleTaskManager<Configuration>(new Factory());

        /// <summary>
        /// Overloaded Role Entry Point Method
        /// </summary>
        public override void Run()
        {
            this.manager.Run();

            base.Run();
        }

        /// <summary>
        /// Overloaded Role Entry Point Method
        /// </summary>
        public override bool OnStart()
        {
            var config = new Configuration()
            {
                ConnectionString = "UseDevelopmentStorage=true;",
                TableName = "table",
                GenericQueueName = "queue",
                ContainerName = "container",
                FastQueueName = "fast",
                ModerateQueueName = "moderate",
                SlowQueueName = "slow",
            };

            return this.manager.OnStart(config);
        }

        /// <summary>
        /// Overloaded Role Entry Point Method
        /// </summary>
        public override void OnStop()
        {
            this.manager.OnStop();

            base.OnStop();
        }
    }
}

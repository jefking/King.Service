namespace Worker
{
    using King.Azure.BackgroundWorker;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class WorkerRole : RoleEntryPoint
    {
        /// <summary>
        /// Role Service Manager
        /// </summary>
        private RoleServiceManager manager = new RoleServiceManager(new Factory());

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
            return this.manager.OnStart();
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
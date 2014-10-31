namespace Worker
{
    using King.Service;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class WorkerRole : RoleEntryPoint
    {
        /// <summary>
        /// Role Service Manager
        /// </summary>
        private RoleTaskManager<object> manager = new RoleTaskManager<object>(new Factory());

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
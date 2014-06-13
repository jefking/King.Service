namespace Worker
{
    using King.Azure.BackgroundWorker;

    public class Task : ScheduledManager
    {
        public Task()
            : base("ConnectionStringKey", 30)
        {
        }

        public override void Run()
        {
            //Task that you want to do
        }
    }
}

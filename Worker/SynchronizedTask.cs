namespace Worker
{
    using King.Azure.BackgroundWorker;

    public class SynchronizedTask : CoordinatedManager
    {
        public SynchronizedTask()
            : base("ConnectionStringKey", 30)
        {
        }

        public override void Run()
        {
            //Task that you want to do
        }
    }
}

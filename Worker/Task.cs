namespace Worker
{
    using King.Azure.BackgroundWorker;

    public class Task : Manager
    {
        public Task()
            : base(10, 60)
        {
        }

        public override void Run()
        {
            //Task that you want to do
        }
    }
}
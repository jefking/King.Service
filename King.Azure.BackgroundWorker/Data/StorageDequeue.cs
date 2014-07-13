namespace King.Azure.BackgroundWorker.Data
{

    public class StorageDequeue<T> : Dequeue<T>
    {
        public StorageDequeue(string name, string connectionString, IProcessor<T> processor, int minimumPeriodInSeconds = 15, int maximumPeriodInSeconds = 300)
            :base(new QueuePoller<T>(name, connectionString), processor, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
    }
}
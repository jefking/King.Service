namespace King.Azure.BackgroundWorker.Data
{
    /// <summary>
    /// Storage Dequeue
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class StorageDequeue<T> : Dequeue<T>
    {
        #region Constructors
        public StorageDequeue(string name, string connectionString, IProcessor<T> processor, int minimumPeriodInSeconds = 15, int maximumPeriodInSeconds = 300)
            :base(new QueuePoller<T>(name, connectionString), processor, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion
    }
}
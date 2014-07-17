namespace Worker.Queue
{
    using King.Azure.BackgroundWorker.Data;

    /// <summary>
    /// Dequeue Task, for company model
    /// </summary>
    public class CompanyDequeuer : StorageDequeue<CompanyModel>
    {
        public CompanyDequeuer(string connectionString)
            : base("queue", connectionString, new CompanyProcessor(), 15, 45)
        {
        }
    }
}
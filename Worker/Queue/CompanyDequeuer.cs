namespace Worker.Queue
{
    using King.Azure.BackgroundWorker.Data;

    public class CompanyDequeuer : StorageDequeue<CompanyModel>
    {
        public CompanyDequeuer(string connectionString)
            : base("queue", connectionString, new CompanyProcessor(), 15, 45)
        {
        }
    }
}
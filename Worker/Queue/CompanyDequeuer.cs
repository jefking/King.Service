namespace Worker.Queue
{
    using King.Azure.BackgroundWorker.Data;

    public class CompanyDequeuer : StorageDequeue<CompanyModel>
    {
        public CompanyDequeuer(string connectionString)
            : base("company", connectionString, new CompanyProcessor())
        {
        }
    }
}
namespace Worker.Queue
{
    using King.Service.Data;

    /// <summary>
    /// Dequeue Task, for company model
    /// </summary>
    public class CompanyDequeuer : StorageDequeue<CompanyModel>
    {
        public CompanyDequeuer(string connectionString)
            : base("queue", connectionString, new CompanyProcessor())
        {
        }
    }
}
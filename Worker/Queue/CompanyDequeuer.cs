namespace Worker.Queue
{
    using King.Azure.Data;
    using King.Service.Data;

    /// <summary>
    /// Dequeue Task, for company model
    /// </summary>
    public class CompanyDequeuer : StorageDequeue<CompanyModel>
    {
        public CompanyDequeuer(IStorageQueue queue)
            : base(queue, new CompanyProcessor())
        {
        }
    }
}
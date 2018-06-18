namespace King.Service.Demo.Tasks
{
    using global::Azure.Data.Wrappers;
    using King.Service.Azure;
    using King.Service.Demo.Models;
    using King.Service.Demo.Processors;

    /// <summary>
    /// Dequeue Task, for company model
    /// </summary>
    public class CompanyDequeuer : StorageDequeue<CompanyModel>
    {
        public CompanyDequeuer(string name, string connection)
            : base(new StorageQueue(name, connection), new CompanyProcessor())
        {
        }
    }
}
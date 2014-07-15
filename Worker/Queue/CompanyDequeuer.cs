namespace Worker.Queue
{
    using King.Azure.BackgroundWorker.Data;

    public class CompanyDequeuer : StorageDequeue<CompanyModel>
    {
        public CompanyDequeuer()
            :base("company", "UseDevelopmentStorage=true", new CompanyProcessor())
        {
        }
    }
}
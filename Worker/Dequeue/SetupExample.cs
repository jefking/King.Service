namespace Worker.Dequeue
{
    using King.Azure.Data;
    using King.Service.Data.Model;
    using Worker.Queue;

    public class SetupExample : QueueSetup<CompanyModel>
    {
        public override IProcessor<CompanyModel> Get()
        {
            return new CompanyProcessor();
        }
    }
}
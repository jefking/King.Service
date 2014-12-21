namespace Worker.Queue
{
    using King.Azure.Data;
    using King.Service.Data;

    public class SetupExample : QueueSetup<CompanyModel>
    {
        public override IProcessor<CompanyModel> Get()
        {
            return new CompanyProcessor();
        }
    }
}
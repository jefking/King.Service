namespace King.Service.CloudService.Role
{
    public class Configuration
    {
        public string ConnectionString
        {
            get;
            set;
        }

        public string TableName
        {
            get;
            set;
        }
        public string QueueName
        {
            get;
            set;
        }
        public string ScalableQueueName
        {
            get;
            set;
        }
        public string DynamicQueueName
        {
            get;
            set;
        }
        public string FactoryQueueName
        {
            get;
            set;
        }
        public string ContainerName
        {
            get;
            set;
        }
    }
}
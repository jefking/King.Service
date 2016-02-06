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
        public string GenericQueueName
        {
            get;
            set;
        }
        public string FastQueueName
        {
            get;
            set;
        }
        public string ModerateQueueName
        {
            get;
            set;
        }
        public string SlowQueueName
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
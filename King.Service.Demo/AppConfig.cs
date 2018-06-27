namespace King.Service.Demo
{
    /// <summary>
    /// Application Configuration Settings
    /// </summary>
    public class AppConfig
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
        public string ShardQueueName
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
namespace Worker
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
        public string ContainerName
        {
            get;
            set;
        }
    }
}
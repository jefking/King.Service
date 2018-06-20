namespace King.Service.Demo
{
    using System.Threading;

    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new AppConfig()
            {
                ConnectionString = "UseDevelopmentStorage=true;",
                TableName = "table",
                GenericQueueName = "queue",
                ContainerName = "container",
                FastQueueName = "fast",
                ModerateQueueName = "moderate",
                SlowQueueName = "slow",
                ShardQueueName = "shard"
            };

            using (var manager = new RoleTaskManager<AppConfig>(new TaskFactory(), new TaskFinderFactory<AppConfig>()))
            {
                manager.OnStart(config);

                manager.Run();

                while (true)
                {
                    Thread.Sleep(1500);
                }
            }
        }
    }
}
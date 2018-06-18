namespace King.Service.Demo
{
    using System;
    using System.Threading;
    using Microsoft.

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

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

            using (var manager = new RoleTaskManager<AppConfig>(new TaskFactory()))
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
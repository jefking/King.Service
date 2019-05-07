namespace King.Service.Demo
{
    using King.Service.Demo.Factories;
    using System;
    using System.Threading;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Load Config
            var config = new AppConfig
            {
                ConnectionString = Environment.GetEnvironmentVariable("connection"),
                TableName = "table",
                GenericQueueName = "queue",
                ContainerName = "container",
                FastQueueName = "fast",
                ModerateQueueName = "moderate",
                SlowQueueName = "slow",
                ShardQueueName = "shard"
            };

            // Construct runtime
            using (var manager = new RoleTaskManager<AppConfig>(new Factory(), new DataGenerationFactory(), new TaskFinderFactory<AppConfig>()))
            {
                // Start runtime
                manager.OnStart(config);

                // Run
                manager.Run();

                // Hang on loaded thread
                while (true)
                {
                    Thread.Sleep(1500);
                }
            }
        }
    }
}
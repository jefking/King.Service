namespace King.Service.Demo
{
    using King.Service.Demo.Factories;
    using System;
    using System.Diagnostics;
    using System.Threading;

    public class Program
    {
        public static void Main(string[] args)
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            foreach (var arg in args)
            {
                Trace.TraceInformation("arg: {0}", arg);
            }

            // Load Config
            var config = new AppConfig
            {
                ConnectionString = args[0],
                TableName = "table",
                GenericQueueName = "queue",
                ContainerName = "container",
                FastQueueName = "fast",
                ModerateQueueName = "moderate",
                SlowQueueName = "slow",
                ShardQueueName = "shard"
            };

            Trace.TraceInformation("Connection String: {0}", config.ConnectionString);

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
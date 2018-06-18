namespace King.Service.Demo
{
    using System;
    using System.Threading;

    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new Configuration();
            {
            };

            using (var manager = new RoleTaskManager<Configuration>(new TaskFactory()))
            {
                manager.OnStart(config);

                manager.Run();

                while (true)
                {
                    Thread.Sleep(1500);
                }
            }

            Console.WriteLine("Hello World!");
        }
    }
}
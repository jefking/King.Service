namespace King.Service.Demo
{
    using System;

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
                    //Thread.
                }
            }
            Console.WriteLine("Hello World!");
        }
    }
}
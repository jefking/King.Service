namespace King.Service.Demo.Tasks
{
    using global::Azure.Data.Wrappers;
    using King.Service;
    using King.Service.Demo.Models;
    using System;

    /// <summary>
    /// Example of Task class which adds a company to a queue
    /// </summary>
    public class CompanyQueuer : RecurringTask
    {
        private int id = 0;
        private readonly IStorageQueue queue = null;

        public CompanyQueuer(string name, string connection)
            :base(5)
        {
            this.queue = new StorageQueue(name, connection);
        }

        public override void Run()
        {
            var company = new CompanyModel()
            {
                Id = Guid.NewGuid(),
                Name = string.Format("company{0}", id),
            };

            var task = this.queue.Send(company);
            task.Wait();

            id++;
        }
    }
}

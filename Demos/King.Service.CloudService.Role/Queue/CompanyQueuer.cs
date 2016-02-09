namespace King.Service.CloudService.Role.Queue
{
    using King.Azure.Data;
    using King.Service;
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
                Name = string.Format("company-{0}", id),
            };

            var task = this.queue.Save(company);
            task.Wait();

            id++;
        }
    }
}

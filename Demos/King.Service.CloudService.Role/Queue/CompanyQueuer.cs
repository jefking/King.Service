namespace King.Service.CloudService.Role.Queue
{
    using System;
    using King.Azure.Data;
    using King.Service;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Newtonsoft.Json;

    /// <summary>
    /// Example of Task class which adds a company to a queue
    /// </summary>
    public class CompanyQueuer : RecurringTask
    {
        private int id = 0;
        private readonly IStorageQueue queue = null;

        public CompanyQueuer(IStorageQueue queue)
            :base(10)
        {
            this.queue = queue;
        }

        public override void Run()
        {
            var company = new CompanyModel()
            {
                Id = Guid.NewGuid(),
                Name = string.Format("company-{0}", id),
            };

            var task = this.queue.Save(new CloudQueueMessage(JsonConvert.SerializeObject(company)));
            task.Wait();

            id++;
        }
    }
}

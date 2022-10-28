namespace King.Service.Demo.Tasks
{
    using King.Service;
    using King.Service.Demo.Models;
    using System;

    /// <summary>
    /// Task submits data to the sharded queue
    /// </summary>
    public class CompanyQueuerSharded : RecurringTask
    {
        private int id = 0;
        private readonly IQueueShardSender<IStorageQueue> queue = null;

        public CompanyQueuerSharded(string name, string connection)
            :base(1)
        {
            this.queue = new StorageQueueShards(name, connection);
        }

        public override void Run()
        {
            var company = new CompanyModel()
            {
                Id = Guid.NewGuid(),
                Name = string.Format("company{0}", id),
            };

            var task = this.queue.Save(company);
            task.Wait();

            id++;
        }
    }
}
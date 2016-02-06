namespace King.Service.CloudService.Role.Queue
{
    using System;
    using King.Azure.Data;
    using King.Service;

    class CompanyQueuerSharded : RecurringTask
    {
        private int id = 0;
        private readonly IQueueShardSender<IStorageQueue> queue = null;

        public CompanyQueuerSharded(string name, string connection)
            :base(10)
        {
            this.queue = new StorageQueueShards(name, connection);
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

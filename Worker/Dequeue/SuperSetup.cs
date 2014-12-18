namespace Worker.Dequeue
{
    using King.Azure.Data;
    using King.Service.Data.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Worker.Queue;

    public class SuperSetup<T> : QueueSetup<CompanyModel>
    {
        public override IProcessor<T> Get()
        {
            return new CompanyProcessor();
        }
    }
}
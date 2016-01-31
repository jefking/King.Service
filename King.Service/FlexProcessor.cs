namespace King.Service
{
    using Azure.Data;
    using Data;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class QueueSetupProcessor<Y, T> : QueueSetup<T>
        where Y : IProcessor<T>, new()
    {
        public override Func<IProcessor<T>> Processor
        {
            get
            {
                return () => { return new Y(); };
            }
        }
    }

    public class HappyProcessor : IProcessor<object>
    {
        public Task<bool> Process(object data)
        {
            throw new NotImplementedException();
        }
    }

    public class TaskFactoryTesting
    {
        public IEnumerable<IRunnable> ALlTasks()
        {
            var df = new DequeueFactory("");

            //SHARD Queue; sender and reciever needs to be setup.

            var qp = new QueueSetupProcessor<HappyProcessor, object>() //Easiest to test.
            {
                Priority = QueuePriority.High,
                Name = "ProcessorInline"
            };

            var qa = new QueueAction<object>()
            {
                Priority = QueuePriority.High,
                Name = "MethodPassed",
                Activity = async (obj) => { return await Task.FromResult<bool>(true); },
            };

            var qs = new QueueSetup<object>()
            {
                Name = "FlexPassed",
                Priority = QueuePriority.High,
                Processor = () => { return  new ActionProcessor<object>(async (obj) => { return await Task.FromResult<bool>(true); }); },
            };
            
            return df.Tasks(new[] { qp, qa, qs });
        }
    }
}
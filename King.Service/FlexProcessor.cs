namespace King.Service
{
    using Azure.Data;
    using Data;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public delegate Task<bool> QueueActivity<T>(T data);

    public class ActionProcessor<T> : IProcessor<T>
    {
        private readonly QueueActivity<T> action;

        public ActionProcessor(QueueActivity<T> action)
        {
            if (null == action)
            {
                throw new ArgumentNullException();
            }

            this.action = action;
        }

        public Task<bool> Process(T data)
        {
            var handle = this.action;
            if (null != handle)
            {
                return handle(data);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }

    public class QueueAction<T> : QueueSetup<T>
    {
        public virtual QueueActivity<T> Action
        {
            get;
            set;
        }

        public override Func<IProcessor<T>> Processor
        {
            get
            {
                return () => { return new ActionProcessor<T>(this.Action); };
            }
        }
    }

    public class QueueProcessor<Y, T> : QueueSetup<T>
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

    public class TaskFactory
    {
        public IEnumerable<IRunnable> ALlTasks()
        {
            var df = new DequeueFactory("");

            //SHARD Queue; sender and reciever needs to be setup.

            var qp = new QueueProcessor<HappyProcessor, object>() //Easiest to test.
            {
                Priority = QueuePriority.High,
                Name = "ProcessorInline"
            };

            var qa = new QueueAction<object>()
            {
                Priority = QueuePriority.High,
                Name = "MethodPassed",
                Action = async (obj) => { return await Task.FromResult<bool>(true); },
            };

            var qs = new QueueSetup<object>()
            {
                Name = "FlexPassed",
                Priority = QueuePriority.High,
                Processor = () => { return  new ActionProcessor<object>(async (obj) => { return await Task.FromResult<bool>(true); }); },
            };
            
            return df.Tasks(qa);
        }

        public void Inline()
        {
            new ActionProcessor<object>(async (obj) => { return await Task.FromResult<bool>(true); } );
        }
        
        public IProcessor<T> Create<T>(QueueActivity<T> action)
        {
            return new ActionProcessor<T>(action);
        }
    }
}
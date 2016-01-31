namespace King.Service
{
    using Azure.Data;
    using Data;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public delegate Task<bool> ProcessorAction<T>(T data);

    public class FlexProcessor<T> : IProcessor<T>
    {
        private readonly ProcessorAction<T> action;

        public FlexProcessor(ProcessorAction<T> action)
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
        public QueueAction()
        {
        }

        public virtual ProcessorAction<T> Action
        {
            get;
            set;
        }

        public override Func<IProcessor<T>> Processor
        {
            get
            {
                return () => { return new FlexProcessor<T>(this.Action); };
            }
        }
    }

    public class QueueProcessor<T, Y> : QueueSetup<T>//Why Can't T be inferred from Y?
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

            var qp = new QueueProcessor<object, HappyProcessor>() //Easiest to test.
            {
                Priority = QueuePriority.High,
                Name = "Killer"
            };

            var qa = new QueueAction<object>()
            {
                Priority = QueuePriority.High,
                Name = "OldQueue",
                Action = async (obj) => { return await Task.FromResult<bool>(true); },
            };

            var qs = new QueueSetup<object>()
            {
                Name = "NewQueue",
                Priority = QueuePriority.High,
                Processor = () => { return  new FlexProcessor<object>(async (obj) => { return await Task.FromResult<bool>(true); }); },
            };
            
            return df.Tasks(qa);
        }


        public void Inline()
        {
            new FlexProcessor<object>(async (obj) => { return await Task.FromResult<bool>(true); } );
        }
        
        public IProcessor<T> Create<T>(ProcessorAction<T> action)
        {
            return new FlexProcessor<T>(action);
        }
    }
}
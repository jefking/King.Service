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

    public class QueueSetupAction<T> : QueueSetup<T>
    {
        private readonly ProcessorAction<T> action;

        public QueueSetupAction(ProcessorAction<T> action)
        {
            if (null == action)
            {
                throw new ArgumentNullException();
            }

            this.action = action;
        }
        
        public override Func<IProcessor<T>> Processor
        {
            get
            {
                return () => { return new FlexProcessor<T>(action); };
            }
        }
    }
    
    public class TaskFactory
    {
        public IEnumerable<IRunnable> ALlTasks()
        {
            var df = new DequeueFactory("");
            var qsa = new QueueSetupAction<object>(async (obj) => { return await Task.FromResult<bool>(true); })
            {
                Priority = QueuePriority.High,
                Name = "happy",
            };
            yield return df.Tasks(qsa);
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
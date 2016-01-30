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
    public class FlexSetup<T> : IQueueSetup<T>
    {
        private readonly ProcessorAction<T> action;

        public FlexSetup(ProcessorAction<T> action)
        {
            if (null == action)
            {
                throw new ArgumentNullException();
            }

            this.action = action;
        }

        public string Name
        {
            get;
            set;
        }

        public QueuePriority Priority
        {
            get;
            set;
        }

        public Func<IProcessor<T>> Processor
        {
            get
            {
                return () => { return new FlexProcessor<T>(action); };
            }
        }
    }

    //public class QueueBinding<T>
    //{
    //    public ProcessorAction<T> Action;
    //    public string QueueName;
    //}


    public class Factory
    {
        public void Inline()
        {
            new FlexProcessor<object>(async (obj) => { return await Task.FromResult<bool>(true); } );
        }

        public IEnumerable<IRunnable> Create<T>(IQueueSetup<T> setup)
        {
            var df = new DequeueFactory();
            return df.Tasks<T>(setup.Name, setup.Processor, setup.Priority);
        }

        public IProcessor<T> Create<T>(ProcessorAction<T> action)
        {
            return new FlexProcessor<T>(action);
        }
    }
}
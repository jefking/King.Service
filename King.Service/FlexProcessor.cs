namespace King.Service
{
    using Azure.Data;
    using System.Threading.Tasks;

    public class FlexProcessor<T> : IProcessor<T>
    {
        public delegate Task<bool> ToProcess<T>(T data);

        private readonly ToProcess<T> action;

        public FlexProcessor(ToProcess<T> action)
        {
            this.action = action;
        }

        public Task<bool> Process(T data)
        {
            return this.action(data);
        }
    }

    public class haha
    {
        public void Blah()
        {
            new FlexProcessor<object>(async (obj) => { return await Task.FromResult<bool>(true); } );
        }
    }
}
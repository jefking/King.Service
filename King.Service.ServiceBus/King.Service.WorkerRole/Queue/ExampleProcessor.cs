namespace King.Service.ServiceBus.Queue
{
    using King.Azure.Data;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Processes dequeued data of type ExampleModel
    /// </summary>
    public class ExampleProcessor : IProcessor<ExampleModel>
    {
        public Task<bool> Process(ExampleModel data)
        {
            Trace.TraceInformation("Polled for: '{0}/{1}'", data.Name, data.Identifier);

            return Task.FromResult<bool>(true);
        }
    }
}
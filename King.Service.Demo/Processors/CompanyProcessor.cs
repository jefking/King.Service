namespace King.Service.Demo.Processors
{
    using global::Azure.Data.Wrappers;
    using King.Service.Demo.Models;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Company Processor, post dequeue action
    /// </summary>
    public class CompanyProcessor : IProcessor<CompanyModel>
    {
        public Task<bool> Process(CompanyModel data)
        {
            Trace.TraceInformation("Save company to data store: '{0}/{1}'", data.Name, data.Id);

            return Task.FromResult<bool>(true);
        }
    }
}
namespace Worker.Scalable
{
    using King.Service;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class ScalableTask : IDynamicRuns
    {
        public int MaximumPeriodInSeconds
        {
            get { return 30; }
        }

        public int MinimumPeriodInSeconds
        {
            get { return 20; }
        }

        public Task<bool> Run()
        {
            Trace.TraceInformation("Scalable Task Running.");
            return Task.FromResult(true);
        }
    }
}
namespace Worker
{
    using King.Service;
    using System.Threading.Tasks;

    public class ScalableTask : IDynamicRuns
    {
        public int MaximumPeriodInSeconds
        {
            get { return 1; }
        }

        public int MinimumPeriodInSeconds
        {
            get { return 10; }
        }

        public Task<bool> Run()
        {
            return Task.FromResult(true);
        }
    }
}

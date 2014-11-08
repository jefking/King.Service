namespace Worker
{
    using King.Service;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class DynamicScaler : AutoScaler<Configuration>
    {
        public DynamicScaler(Configuration config)
            : base(1, 15, config)
        {
            Trace.TraceInformation("Scaler Loaded.");
        }

        public override IEnumerable<IScalable> ScaleUnit(Configuration data)
        {
            Trace.TraceInformation("Scaling up.");
            yield return new AdaptiveRunner(new ScalableTask());
            yield return new BackoffRunner(new ScalableTask());
        }
    }
}

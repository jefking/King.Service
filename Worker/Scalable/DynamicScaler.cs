namespace Worker
{
    using King.Service;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class DynamicScaler : AutoScaler<Configuration>
    {
        public DynamicScaler(Configuration config)
            : base(config, 1, 15)
        {
            Trace.TraceInformation("Scaler Loaded.");
        }

        public override IEnumerable<IScalable> ScaleUnit(Configuration data)
        {
            Trace.TraceInformation("Scaling up.");
            yield return new AdaptiveRunner(new ScalableTask());
        }
    }
}

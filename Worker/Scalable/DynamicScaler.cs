namespace Worker.Scalable
{
    using King.Service;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class DynamicScaler : AutoScaler<Configuration>
    {
        public DynamicScaler(Configuration config)
            : base(config, 1, 15, 1)
        {
            Trace.TraceInformation("Scaler Loaded.");
        }

        public override IEnumerable<IScalable> ScaleUnit(Configuration data)
        {
            yield return new AdaptiveRunner(new ScalableTask());
            yield return new BackoffRunner(new ScalableTask()); // More Likely to scale up
        }
    }
}
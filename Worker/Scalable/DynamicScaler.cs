namespace Worker
{
    using King.Service;
    using System.Collections.Generic;

    public class DynamicScaler : AutoScaler<Configuration>
    {
        public DynamicScaler(Configuration config)
            : base(1, 15, config)
        { }

        public override IEnumerable<IScalable> ScaleUnit(Configuration data)
        {
            yield return new AdaptiveRunner(new ScalableTask());
            yield return new BackoffRunner(new ScalableTask());
        }
    }
}

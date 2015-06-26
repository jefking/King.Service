namespace King.Service.CloudService.Role.Scalable
{
    using System.Collections.Generic;
    using King.Service;

    public class DynamicScaler : AutoScaler<Configuration>
    {
        public DynamicScaler(Configuration config)
            : base(config, 1, 15, 1)
        {
        }

        public override IEnumerable<IScalable> ScaleUnit(Configuration data)
        {
            //A pair of tasks that should scale together.
            yield return new AdaptiveRunner(new ScalableTask());
            yield return new BackoffRunner(new ScalableTask());
        }
    }
}
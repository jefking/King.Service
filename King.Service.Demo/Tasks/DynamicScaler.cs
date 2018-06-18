namespace King.Service.Demo.Tasks
{
    using System.Collections.Generic;
    using King.Service;

    public class DynamicScaler : AutoScaler<AppConfig>
    {
        public DynamicScaler(AppConfig config)
            : base(config, 1, 15, 1)
        {
        }

        public override IEnumerable<IScalable> ScaleUnit(AppConfig data)
        {
            //A pair of tasks that should scale together.
            yield return new AdaptiveRunner(new ScalableTask());
            yield return new BackoffRunner(new ScalableTask());
        }
    }
}
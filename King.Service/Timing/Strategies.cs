namespace King.Service.Timing
{
    using System;

    /// <summary>
    /// Timing Strategy Factory
    /// </summary>
    public static class Strategies
    {
        #region Methods
        /// <summary>
        /// Get Calculate Timing
        /// </summary>
        /// <param name="strategy">Timing Strategy</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        /// <returns></returns>
        public static ICalculateTiming Get(Strategy strategy = Strategy.Exponential, int minimumPeriodInSeconds = BaseTimes.MinimumTiming, int maximumPeriodInSeconds = BaseTimes.MaximumTiming)
        {
            switch (strategy)
            {
                case Strategy.Exponential:
                    return new ExponentialTiming(minimumPeriodInSeconds, maximumPeriodInSeconds);
                case Strategy.Linear:
                    return new LinearTiming(minimumPeriodInSeconds, maximumPeriodInSeconds);
                default:
                    throw new InvalidOperationException("Unknown timing strategy.");
            }
        }

        /// <summary>
        /// Get Adaptive Timing
        /// </summary>
        /// <param name="strategy">Timing Strategy</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        /// <returns></returns>
        public static IDynamicTiming Adaptive(Strategy strategy = Strategy.Exponential, int minimumPeriodInSeconds = BaseTimes.MinimumTiming, int maximumPeriodInSeconds = BaseTimes.MaximumTiming)
        {
            return new AdaptiveTiming(Strategies.Get(strategy, minimumPeriodInSeconds, maximumPeriodInSeconds));
        }

        /// <summary>
        /// Get Backoff Timing
        /// </summary>
        /// <param name="strategy">Timing Strategy</param>
        /// <param name="minimumPeriodInSeconds">Minimum Period In Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period In Seconds</param>
        /// <returns></returns>
        public static IDynamicTiming Backoff(Strategy strategy = Strategy.Exponential, int minimumPeriodInSeconds = BaseTimes.MinimumTiming, int maximumPeriodInSeconds = BaseTimes.MaximumTiming)
        {
            return new BackoffTiming(Strategies.Get(strategy, minimumPeriodInSeconds, maximumPeriodInSeconds));
        }
        #endregion
    }
}
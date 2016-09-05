namespace King.Service
{
    using System;
    using Timing;

    /// <summary>
    /// Remarks:
    /// Must be placed on class with a default constructor
    /// Must be placed on a method with no parameters
    /// Must be placed on method that returns bool (work was done)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class HappensBetweenAttribute : Attribute
    {
        #region Members
        /// <summary>
        /// Frequency Range
        /// </summary>
        public Range<int> Frequency
        {
            get;
            protected set;
        }

        /// <summary>
        /// Timing Strategy
        /// </summary>
        public Strategy Strategy
        {
            get;
            protected set;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="minimumPeriodInSeconds">Minimum Period in Seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum Period in Seconds</param>
        /// <param name="strategy">Timing Strategy</param>
        public HappensBetweenAttribute(int minimumPeriodInSeconds = BaseTimes.DefaultMinimumTiming, int maximumPeriodInSeconds = BaseTimes.DefaultMaximumTiming, Strategy strategy = Strategy.Exponential)
        {
            if (0 >= minimumPeriodInSeconds)
            {
                throw new ArgumentException("Minimum frequency must be greater than 0.");
            }
            if (minimumPeriodInSeconds > maximumPeriodInSeconds)
            {
                throw new ArgumentException("Maximum frequency must be greater than minimum.");
            }

            this.Frequency = new Range<int>(minimumPeriodInSeconds, maximumPeriodInSeconds);
            this.Strategy = strategy;
        }
        #endregion
    }
}
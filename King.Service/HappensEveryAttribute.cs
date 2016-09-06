namespace King.Service
{
    using System;
    using Timing;

    /// <summary>
    /// Remarks:
    /// Must be placed on class with a default constructor
    /// Must be placed on a method with no parameters
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class HappensEveryAttribute : Attribute
    {
        #region Members
        /// <summary>
        /// Frequency in Seconds
        /// </summary>
        public virtual int Frequency
        {
            get;
            protected set;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="frequency">Frequency in Seconds</param>
        public HappensEveryAttribute(int frequency = BaseTimes.DefaultMaximumTiming)
        {
            if (0 >= frequency)
            {
                throw new ArgumentException("Frequency must be greater than 0.");
            }

            this.Frequency = frequency;
        }
        #endregion
    }
}
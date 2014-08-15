namespace King.Service
{
    using System;

    /// <summary>
    /// Timing Maths
    /// </summary>
    public class Timing : ITiming
    {
        #region Methods
        /// <summary>
        /// Exponential Backoff strategy, within bounds
        /// </summary>
        /// <param name="attempts">attempts</param>
        /// <param name="max">upper bound</param>
        /// <param name="min">lower bound</param>
        /// <returns>timing</returns>
        public virtual double Exponential(ulong attempts, int max, int min = 1)
        {
            if (0 == attempts)
            {
                return min;
            }

            var current = ((Math.Pow(2, attempts) * .1d) * min) + min;
            return current < max ? current : max;
        }
        #endregion
    }
}
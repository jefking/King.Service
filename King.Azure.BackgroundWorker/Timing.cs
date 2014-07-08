namespace King.Azure.BackgroundWorker
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
        /// <param name="min">lower bound</param>
        /// <param name="max">upper bound</param>
        /// <param name="attempts">attempts</param>
        /// <returns>timing</returns>
        public double Exponential(int min = 0, int max = 60, ulong attempts = 0)
        {
            if (0 == attempts)
            {
                return min;
            }

            var current = Math.Pow(2, attempts);
            return current < max ? current : max;
        }
        #endregion
    }
}
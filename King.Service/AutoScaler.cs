namespace King.Service
{
    using System;

    /// <summary>
    /// Task AutoScaler
    /// </summary>
    public class AutoScaler : IAutoScaler
    {
        #region Members
        /// <summary>
        /// Minimum Instance Count
        /// </summary>
        protected readonly byte minimum = 0;

        /// <summary>
        /// Maximum Instance Count
        /// </summary>
        protected readonly byte maximum = 1;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="minimum">Minimum</param>
        /// <param name="maximum">Maximum</param>
        public AutoScaler(byte minimum = 0, byte maximum = 1)
        {
            if (minimum > maximum)
            {
                throw new ArgumentException("Minimum should be less than Maximum");
            }

            this.minimum = minimum;
            this.maximum = maximum;
        }
        #endregion

        #region
        /// <summary>
        /// Minimum Instance Count
        /// </summary>
        public byte Minimum
        {
            get
            {
                return this.minimum;
            }
        }

        /// <summary>
        /// Maximum Instance Count
        /// </summary>
        public byte Maximum
        {
            get
            {
                return this.maximum;
            }
        }
        #endregion
    }
}
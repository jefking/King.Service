namespace King.Service
{
    /// <summary>
    /// Range
    /// </summary>
    /// <typeparam name="X">Type</typeparam>
    public struct Range<X>
    {
        #region Constructor
        /// <summary>
        /// Range Constructor
        /// </summary>
        /// <param name="min">Minimum</param>
        /// <param name="max">Maximum</param>
        public Range(X min, X max)
        {
            this.Minimum = min;
            this.Maximum = max;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Minimum
        /// </summary>
        public X Minimum
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum
        /// </summary>
        public X Maximum
        {
            get;
            set;
        }
        #endregion
    }
}
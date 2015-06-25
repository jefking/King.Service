namespace King.Service
{
    /// <summary>
    /// Range
    /// </summary>
    /// <typeparam name="X"></typeparam>
    public class Range<X>
    {
        #region Properties
        /// <summary>
        /// Minimum
        /// </summary>
        public virtual X Minimum
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum
        /// </summary>
        public virtual X Maximum
        {
            get;
            set;
        }
        #endregion
    }
}
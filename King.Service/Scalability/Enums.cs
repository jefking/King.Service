namespace King.Service.Scalability
{
    /// <summary>
    /// Scale Direction
    /// </summary>
    public enum Direction : sbyte
    {
        /// <summary>
        /// No Scaling
        /// </summary>
        None = 0,
        /// <summary>
        /// Scale Down
        /// </summary>
        Down = -1,
        /// <summary>
        /// Scale Up
        /// </summary>
        Up = 1,
    }
}
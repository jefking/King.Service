namespace King.Service.Data
{
    /// <summary>
    /// Queue Priority
    /// </summary>
    /// <remarks>
    /// This shapes
    /// - Timing Strategy
    /// - Concurrency
    /// - Cost
    /// - Throughput
    /// Default = Low
    /// </remarks>
    public enum QueuePriority : byte
    {
        Low = 0,
        Medium = 1,
        High = 2,
    }
}
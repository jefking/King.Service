namespace King.Service.Data
{
    using System.Threading.Tasks;

    /// <summary>
    /// Queue Activity
    /// </summary>
    /// <typeparam name="T">Data to be processed</typeparam>
    /// <param name="data">Payload</param>
    /// <returns>Async Task</returns>
    public delegate Task<bool> QueueActivity<T>(T data);
}
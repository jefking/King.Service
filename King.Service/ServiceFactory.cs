namespace King.Service
{
    using System.Collections.Generic;

    /// <summary>
    /// Service Factory
    /// </summary>
    public abstract class ServiceFactory : IServiceFactory<object>
    {
        #region Methods
        public abstract IEnumerable<IRunnable> Services(object passthrough);
        #endregion
    }
}
namespace King.Service.CloudService.Role.Queue
{
    using System;

    /// <summary>
    /// Example model for queuing
    /// </summary>
    [Serializable]
    public class CompanyModel
    {
        public Guid Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
    }
}

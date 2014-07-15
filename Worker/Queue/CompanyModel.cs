namespace Worker.Queue
{
    using System;

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

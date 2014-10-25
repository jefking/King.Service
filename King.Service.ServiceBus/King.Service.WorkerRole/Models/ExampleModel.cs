namespace King.Service.ServiceBus.Queue
{
    using System;

    [Serializable]
    public class ExampleModel
    {
        public Guid Identifier
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
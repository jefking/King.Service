namespace King.Service.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AdaptiveHelper : AdaptiveTask
    {
        public bool Work
        {
            get;
            set;
        }
        public override void Run(out bool workWasDone)
        {
            workWasDone = this.Work;
        }
    }
}

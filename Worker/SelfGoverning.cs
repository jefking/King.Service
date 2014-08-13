namespace Worker
{
    using King.Service;
    using System;

    public class SelfGoverning : SelfGoverningTask
    {
        public override void Run(out bool workWasDone)
        {
            workWasDone = (DateTime.Now.Ticks % 2) == 0;
        }
    }
}
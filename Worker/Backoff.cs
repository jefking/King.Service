namespace Worker
{
    using King.Service;

    public class Backoff : BackoffTask
    {
        private int work = 0;

        public override void Run(out bool workWasDone)
        {
            workWasDone = false;

            if (work > 3)
            {
                //If data processing needed to occur:
                workWasDone = true;

                work = 0;
            }
            else
            {
                //No data processing occurred.
                work++;
            }
        }
    }
}
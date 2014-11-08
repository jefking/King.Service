namespace King.Service.Tests
{
    using System;

    public class RecurringHelper : RecurringTask
    {
        public RecurringHelper(int dueInSeconds, int periodInSeconds)
            : base(dueInSeconds, periodInSeconds)
        {
        }
        public bool Throw
        {
            get;
            set;
        }
        public override void Run()
        {
            if (this.Throw)
            {
                throw new InvalidOperationException();
            }
        }
        public void TestDispose()
        {
            base.Dispose(true);
        }

        public void Change(TimeSpan st)
        {
            base.ChangeTiming(st);
        }
    }
}

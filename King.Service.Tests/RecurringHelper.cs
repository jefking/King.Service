namespace King.Service.Tests
{
    using System;

    public class RecurringHelper : RecurringTask
    {
        public RecurringHelper(int periodInSeconds)
            : base(periodInSeconds)
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

        public void ChangeTiming(TimeSpan ts)
        {
            base.Change(ts);
        }
    }
}
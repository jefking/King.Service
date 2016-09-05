namespace King.Service
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using Timing;

    /// <summary>
    /// Remarks:
    /// Must be placed on class with a default constructor
    /// Must be placed on a method with no parameters
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class HappensEveryAttribute : Attribute
    {
        public int Frequency
        {
            get;
            private set;
        }
        public HappensEveryAttribute(int frequency = BaseTimes.DefaultMaximumTiming)
        {
            this.Frequency = frequency;
        }
    }

    /// <summary>
    /// Remarks:
    /// Must be placed on class with a default constructor
    /// Must be placed on a method with no parameters
    /// Must be placed on method that returns bool
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class HappensBetweenAttribute : Attribute
    {
        public Range<int> Frequency
        {
            get;
            private set;
        }
        public Strategy Strategy
        {
            get;
            private set;
        }
        public HappensBetweenAttribute(int minimumPeriodInSeconds = BaseTimes.DefaultMinimumTiming, int maximumPeriodInSeconds = BaseTimes.DefaultMaximumTiming, Strategy strategy = Strategy.Exponential)
        {
            this.Frequency = new Range<int>(minimumPeriodInSeconds, maximumPeriodInSeconds);
            this.Strategy = Strategy;
        }

        public ICalculateTiming Get()
        {
            return Strategies.Get(this.Strategy, this.Frequency.Minimum, this.Frequency.Maximum);
        }
    }

    public class Finder<T> : ITaskFactory<T>
    {
        public IEnumerable<IRunnable> Tasks(T passthrough)
        {
            var t = typeof(T);

            var assembly = t.GetTypeInfo().Assembly;

            var types = assembly.DefinedTypes;

            var runnables = new List<IRunnable>();
            Parallel.ForEach(types, type =>
            {
                foreach (var method in type.DeclaredMethods)
                {
                    foreach (var attr in method.GetCustomAttributes(typeof(HappensEveryAttribute), false))
                    {
                        var every = attr as HappensEveryAttribute;
                        var c = Activator.CreateInstance(type.DeclaringType);
                        var run = new BindRuns(c, method, every.Frequency);
                        var recurring = new RecurringRunner(run);
                        runnables.Add(recurring);
                    }
                    foreach (var attr in method.GetCustomAttributes(typeof(HappensBetweenAttribute), false))
                    {
                        var between = attr as HappensBetweenAttribute;
                        var c = Activator.CreateInstance(type.DeclaringType);
                        var run = new BetweenRuns(c, method, between.Frequency.Minimum, between.Frequency.Maximum);
                        var recurring = new AdaptiveRunner(run, between.Strategy);//Choice
                        runnables.Add(recurring);
                    }
                }
            });

            return runnables;
        }
    }

    public class BindRuns : IRuns
    {
        public int MinimumPeriodInSeconds
        {
            get;
            private set;
        }

        private readonly object instance;
        private readonly MethodInfo method;

        public BindRuns(object instance, MethodInfo method, int min)
        {
            this.instance = instance;
            this.method = method;
            this.MinimumPeriodInSeconds = min;
        }

        public Task<bool> Run()
        {
            this.method.Invoke(instance, null);
            return Task.FromResult<bool>(true);
        }
    }
    public class BetweenRuns : IDynamicRuns
    {
        public int MinimumPeriodInSeconds
        {
            get;
            private set;
        }

        public int MaximumPeriodInSeconds
        {
            get;
            private set;
        }

        private readonly object instance;
        private readonly MethodInfo method;

        public BetweenRuns(object instance, MethodInfo method, int min, int max)
        {
            this.instance = instance;
            this.method = method;
            this.MinimumPeriodInSeconds = min;
            this.MaximumPeriodInSeconds = max;
        }

        public Task<bool> Run()
        {
            var result = this.method.Invoke(instance, null);
            return Task.FromResult<bool>((bool)result);
        }
    }

    public class ExampleTest
    {
        [HappensEvery(100)]
        public void ActionRecurring() { }

        [HappensBetween(1, 100, Strategy.Linear)]
        public bool ActionDynamic() { return true; }
    }
}
namespace King.Service
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading.Tasks;
    using Timing;

    /// <summary>
    /// Task Finder, searches assembly for attribute based tasks.
    /// </summary>
    /// <typeparam name="T">Object in Assembly to look for tasks.</typeparam>
    public class TaskFinderFactory<T> : ITaskFactory<T>
    {
        #region Methods
        /// <summary>
        /// Attribute Task Manifest
        /// </summary>
        /// <param name="passthrough">Configuration</param>
        /// <returns>Runnable Tasks</returns>
        public virtual IEnumerable<IRunnable> Tasks(T passthrough)
        {
            var assembly = typeof(T).GetTypeInfo().Assembly;

            var runnables = new List<IRunnable>();
            Parallel.ForEach(assembly.DefinedTypes, type =>
            {
                foreach (var method in type.DeclaredMethods)
                {
                    foreach (var everyAttr in method.GetCustomAttributes(typeof(InitializeAttribute), false))
                    {
                        Trace.TraceInformation("Initialization found: {0}.{1}", type.FullName, method.Name);

                        var instance = Activator.CreateInstance(type.AsType());
                        var recurring = new InitializeRunner(instance, method);
                        runnables.Add(recurring);
                    }

                    foreach (var everyAttr in method.GetCustomAttributes(typeof(RunsEveryAttribute), false))
                    {
                        Trace.TraceInformation("Runs Every found: {0}.{1}", type.FullName, method.Name);
                        
                        var every = everyAttr as RunsEveryAttribute;
                        var instance = Activator.CreateInstance(type.AsType());
                        var run = new EveryRuns(instance, method, every.Frequency);
                        var recurring = new RecurringRunner(run);
                        runnables.Add(recurring);
                    }

                    foreach (var betweenAttr in method.GetCustomAttributes(typeof(RunsBetweenAttribute), false))
                    {
                        Trace.TraceInformation("Runs between found: {0}.{1}", type.FullName, method.Name);

                        var between = betweenAttr as RunsBetweenAttribute;
                        var instance = Activator.CreateInstance(type.AsType());
                        var run = new BetweenRuns(instance, method, between.Frequency.Minimum, between.Frequency.Maximum);
                        switch (between.Strategy)
                        {
                            case Strategy.Exponential:
                                runnables.Add(new BackoffRunner(run, between.Strategy));
                                break;
                            case Strategy.Linear:
                            default:
                                runnables.Add(new AdaptiveRunner(run, between.Strategy));
                                break;
                        }
                    }
                }
            });

            return runnables;
        }
        #endregion
    }
}
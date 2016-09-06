namespace King.Service
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Task Finder, searches assembly for attribute based tasks.
    /// </summary>
    /// <typeparam name="T">Object in Assembly to look for tasks.</typeparam>
    public class TaskFinder<T> : ITaskFactory<T>
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
                    foreach (var everyAttr in method.GetCustomAttributes(typeof(RunsEveryAttribute), false))
                    {
                        var every = everyAttr as RunsEveryAttribute;
                        var instance = Activator.CreateInstance(type.DeclaringType);
                        var run = new EveryRuns(instance, method, every.Frequency);
                        var recurring = new RecurringRunner(run);
                        runnables.Add(recurring);
                    }

                    foreach (var betweenAttr in method.GetCustomAttributes(typeof(RunsBetweenAttribute), false))
                    {
                        var between = betweenAttr as RunsBetweenAttribute;
                        var instance = Activator.CreateInstance(type.DeclaringType);
                        var run = new BetweenRuns(instance, method, between.Frequency.Minimum, between.Frequency.Maximum);
                        var recurring = new AdaptiveRunner(run, between.Strategy);//Choice, Adaptive/Backoff
                        runnables.Add(recurring);
                    }
                }
            });

            return runnables;
        }
        #endregion
    }
}
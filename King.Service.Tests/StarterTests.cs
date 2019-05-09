namespace King.Service.Tests
{
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class StarterTests
    {
        [Test]
        public void Constructor()
        {
            new Starter();
        }

        [Test]
        public void IsIStarter()
        {
            Assert.IsNotNull(new Starter() as IStarter);
        }

        [Test]
        public void IsIDisposable()
        {
            Assert.IsNotNull(new Starter() as IDisposable);
        }

        [Test]
        public void StartNull()
        {
            var s = new Starter();
            s.Start(null);
        }

        [Test]
        public void StartNone()
        {
            var s = new Starter();
            s.Start(new List<IRunnable>());
        }

        [Test]
        public void Start()
        {
            var tasks = new List<IRunnable>();
            var task = Substitute.For<IRunnable>();
            tasks.Add(task);

            var s = new Starter();
            s.Start(tasks);
        }

        [Test]
        public void StartTasksNull()
        {
            var s = new Starter();
            s.StartTask(null);
        }

        [Test]
        public void StartTasksNone()
        {
            var s = new Starter();
            s.StartTask(new List<IRunnable>());
        }

        [Test]
        public void StartTasksUnknown()
        {
            var s = new Starter();
            s.StartTask(new object());
        }

        [Test]
        public void StartTask()
        {
            var tasks = new Stack<IRunnable>();
            var task = Substitute.For<IRunnable>();
            tasks.Push(task);

            var s = new Starter();
            s.StartTask(tasks);

            task.Received().Start();
        }

        [Test]
        public void StartTaskThrows()
        {
            var tasks = new Stack<IRunnable>();
            var task = Substitute.For<IRunnable>();
            task.When(t => t.Start()).Do(x => { throw new Exception(); });
            tasks.Push(task);

            var s = new Starter();
            s.StartTask(tasks);

            task.Received().Start();
        }
    }
}
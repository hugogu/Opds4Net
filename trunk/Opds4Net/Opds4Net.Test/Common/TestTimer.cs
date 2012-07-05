using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Opds4Net.Test.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class TestTimer
    {
        private readonly Action action;
        private readonly Stopwatch timer = new Stopwatch();
        private long times = 0;

        public TestTimer(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            this.action = action;
        }

        /// <summary>
        /// Test how many times an operation could perform within a given time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public long TimesInTime(TimeSpan time)
        {
            action();

            times = 0;
            timer.Restart();
            while (timer.Elapsed < time)
            {
                action();
                times++;
            }
            timer.Stop();

            return times;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="workerCount"></param>
        /// <returns></returns>
        public long TimesInTimeParallel(TimeSpan time, int workerCount)
        {
            if (workerCount <= 0 || workerCount > 64)
                throw new ArgumentOutOfRangeException("workerCount", "wokerCount must between 1 and 64");

            action();
            times = 0;
            var cts = new CancellationTokenSource();
            var factory = new TaskFactory();
            var tasks = new List<Task>(workerCount);
            for (int i = 0; i < workerCount; i++)
            {
                tasks.Add(factory.StartNew(TimesParallelWorkerAction, cts));
            }

            Thread.Sleep(time);
            cts.Cancel();

            return times;
        }

        /// <summary>
        /// Test the time an operation could take to run specified times.
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public TimeSpan TimeForTimes(int times)
        {
            if (times <= 0)
                throw new ArgumentOutOfRangeException("times");

            // Run a time to worm up.
            action();

            timer.Restart();
            var counter = 0;
            while (counter++ < times)
            {
                action();
            }
            timer.Stop();

            return timer.Elapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="times"></param>
        /// <param name="workerCount"></param>
        /// <returns></returns>
        public TimeSpan TimeForTimesParallel(int times, int workerCount)
        {
            if (times <= 0)
                throw new ArgumentOutOfRangeException("times");
            if (workerCount <= 0 || workerCount > 64)
                throw new ArgumentOutOfRangeException("workerCount", "wokerCount must between 1 and 64");

            action();
            this.times = times;
            var tasks = new List<Task>(workerCount);
            for (int i = 0; i < workerCount; i++)
            {
                tasks.Add(new Task(TimeParallelWorkerAction));
            }

            tasks.ForEach(t => t.Start());
            timer.Restart();
            Task.WaitAll(tasks.ToArray());
            timer.Stop();

            return timer.Elapsed;
        }

        private void TimeParallelWorkerAction()
        {
            while (times > 0)
            {
                action();
                Interlocked.Decrement(ref times);
            }
        }

        private void TimesParallelWorkerAction(object state)
        {
            var cts = state as CancellationTokenSource;
            while (!cts.IsCancellationRequested)
            {
                action();
                Interlocked.Increment(ref times);
            }
        }
    }
}

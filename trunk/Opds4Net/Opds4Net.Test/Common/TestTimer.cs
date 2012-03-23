using System;
using System.Diagnostics;

namespace Opds4Net.Test.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class TestTimer
    {
        private Action action;
        private Stopwatch timer = new Stopwatch();

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

            long i = 0;
            timer.Restart();
            while (timer.Elapsed < time)
            {
                action();
                i++;
            }
            timer.Stop();

            return i;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public long TimesInTimeParallel(TimeSpan time)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Test the time an operation could take to run specified times.
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public TimeSpan Run(int times)
        {
            // Run a time to worm up.
            action();

            timer.Restart();
            int counter = 0;
            while (counter++ < times)
            {
                action();
            }
            timer.Stop();

            return timer.Elapsed;
        }
    }
}

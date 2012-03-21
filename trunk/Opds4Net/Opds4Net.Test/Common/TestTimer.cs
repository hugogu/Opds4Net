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

using System;
using System.Threading;

namespace Solidry.Aspects
{
    public abstract class WithAspectAndRetry<TInput, TOutput>: WithAspect<TInput, TOutput>
    {
        private readonly int _delayMiliseconds;

        /// <summary>
        /// Retry with dealy miliseconds.
        /// </summary>
        /// <param name="delayMiliseconds"></param>
        protected WithAspectAndRetry(int delayMiliseconds)
        {
            _delayMiliseconds = delayMiliseconds;
        }

        /// <summary>
        /// Retry with delay.
        /// </summary>
        /// <param name="delay"></param>
        protected WithAspectAndRetry(TimeSpan delay) : this((int)delay.TotalMilliseconds)
        {

        }

        /// <summary>
        /// Retry with 100 miliseconds.
        /// </summary>
        protected WithAspectAndRetry() : this(100)
        {
        }

        /// <summary>
        /// Invoke logic with retry.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected new TOutput Invoke(TInput input)
        {
            int attempt = 0;
            int delayMiliseconds = _delayMiliseconds;

            while (true)
            {
                attempt++;

                try
                {
                    return base.Invoke(input);
                }
                catch (Exception e)
                {
                    if (!Retry(e, attempt, delayMiliseconds, x => { delayMiliseconds = x; }))
                    {
                        throw;
                    }
                }

                SpinWait.SpinUntil(() => false, delayMiliseconds);
            }
        }

        /// <summary>
        /// Check if retry logic.
        /// </summary>
        /// <param name="ex">Exception threw by logic.</param>
        /// <param name="attempt">Number of attempts executing logic.</param>
        /// <param name="currentDelay">Current delay.</param>
        /// <param name="setNewDelay">Method to set new delay in method scope.</param>
        /// <returns>If true then retry.</returns>
        protected abstract bool Retry(Exception ex, int attempt, int currentDelay, Action<int> setNewDelay);
    }
}
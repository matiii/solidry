using System;
using System.Collections.Generic;
using System.Threading;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Internal;

namespace Solidry.Aspects
{
    /// <summary>
    /// Aspect with retry.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class WithAspectAndRetry<TInput, TOutput>: WithAspect<TInput, TOutput>
    {
        private readonly IRetryStrategy _retryStrategy;
        private readonly int _delayMiliseconds;

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, delay and general aspect.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="delay"></param>
        /// <param name="generalAspect"></param>
        protected WithAspectAndRetry(
            IRetryStrategy retryStrategy,
            int delay,
            IGeneralAspect generalAspect): 
            this(TimeSpan.FromMilliseconds(delay), retryStrategy, generalAspect, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, 20ms delay and general aspect.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        protected WithAspectAndRetry(
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect) :
            this(TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, generalAspect, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, delay and before aspect.
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="before"></param>
        /// <param name="retryStrategy"></param>
        protected WithAspectAndRetry(
            IRetryStrategy retryStrategy,
            int delay,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before): 
            this(TimeSpan.FromMilliseconds(delay), retryStrategy, null, before, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, 20ms delay and before aspect.
        /// </summary>
        /// <param name="before"></param>
        /// <param name="retryStrategy"></param>
        protected WithAspectAndRetry(
            IRetryStrategy retryStrategy,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before) :
            this(TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, null, before, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, delay, before and after aspect.
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <param name="retryStrategy"></param>
        protected WithAspectAndRetry(
            IRetryStrategy retryStrategy,
            int delay,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before,
            IReadOnlyList<IAfterAspect<TInput, TOutput>> after): 
            this(TimeSpan.FromMilliseconds(delay), retryStrategy, null, before, after)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, 20ms delay, before and after aspect.
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <param name="retryStrategy"></param>
        protected WithAspectAndRetry(
            IRetryStrategy retryStrategy,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before,
            IReadOnlyList<IAfterAspect<TInput, TOutput>> after) :
            this(TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, null, before, after)
        {
        }

        /// <summary>
        /// Create with delay, retry strategy, general, before and after aspect.
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        protected WithAspectAndRetry(
            TimeSpan delay,
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before,
            IReadOnlyList<IAfterAspect<TInput, TOutput>> after): 
            base(generalAspect, before, after)
        {
            _retryStrategy = retryStrategy;
            _delayMiliseconds = (int) delay.TotalMilliseconds;
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
                    if (!_retryStrategy.Retry(CurrentOperationId, e, attempt, delayMiliseconds, x => { delayMiliseconds = x; }))
                    {
                        throw;
                    }
                }

                SpinWait.SpinUntil(() => false, delayMiliseconds);
            }
        }
    }
}
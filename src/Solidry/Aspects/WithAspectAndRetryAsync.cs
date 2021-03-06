﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Contract.Factory;
using Solidry.Aspects.Internal;

namespace Solidry.Aspects
{
    /// <inheritdoc />
    /// <summary>
    /// Asynchronous aspect with retry.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class WithAspectAndRetryAsync<TInput, TOutput> : WithAspectAsync<TInput, TOutput>
    {
        private readonly IRetryStrategy _retryStrategy;
        private readonly int _delayMiliseconds;

        /// <inheritdoc />
        /// <summary>
        /// Create with delay, retry strategy, general aspect, asynchronous aspect, asynchronous before and after aspect.
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        /// <param name="afterAsync"></param>
        protected WithAspectAndRetryAsync(
            TimeSpan delay,
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync,
            IReadOnlyList<IAfterAspectAsync<TInput, TOutput>> afterAsync) 
            :base(generalAspect, generalAspectAsync, beforeAsync, afterAsync)
        {
            _retryStrategy = retryStrategy;
            _delayMiliseconds = (int) delay.TotalMilliseconds;
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with factory.
        /// </summary>
        /// <param name="factory"></param>
        protected WithAspectAndRetryAsync(IAspectAndRetryAsyncFactory<TInput, TOutput> factory)
            : this(factory.Delay, factory.RetryStrategy, factory.GeneralAspect, factory.GeneralAspectAsync, factory.BeforeAsync, factory.AfterAsync)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with 20ms delay, retry strategy, general aspect, asynchronous aspect, asynchronous before and after aspect.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        /// <param name="afterAsync"></param>
        protected WithAspectAndRetryAsync(
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync,
            IReadOnlyList<IAfterAspectAsync<TInput, TOutput>> afterAsync)
            : this(TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, generalAspect, generalAspectAsync, beforeAsync, afterAsync)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, delay and general aspect.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="delay"></param>
        /// <param name="generalAspect"></param>
        protected WithAspectAndRetryAsync(
            IRetryStrategy retryStrategy,
            int delay,
            IGeneralAspect generalAspect) 
            :this(TimeSpan.FromMilliseconds(delay), retryStrategy, generalAspect, null, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, 20ms delay and general aspect.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        protected WithAspectAndRetryAsync(
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect)
            : this(TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, generalAspect, null, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, delay, general aspect and asynchronous general aspect.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="delay"></param>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        protected WithAspectAndRetryAsync(
            IRetryStrategy retryStrategy,
            int delay,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync)
            : this(TimeSpan.FromMilliseconds(delay), retryStrategy, generalAspect, generalAspectAsync, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, 20ms delay, general aspect and asynchronous general aspect.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        protected WithAspectAndRetryAsync(
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync)
            : this(TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, generalAspect, generalAspectAsync, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, delay, general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="delay"></param>
        /// <param name="generalAspect"></param>
        /// <param name="beforeAsync"></param>
        protected WithAspectAndRetryAsync(
            IRetryStrategy retryStrategy,
            int delay,
            IGeneralAspect generalAspect,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync)
            : this(TimeSpan.FromMilliseconds(delay), retryStrategy, generalAspect, null, beforeAsync, null)

        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, 20ms delay, general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="beforeAsync"></param>
        protected WithAspectAndRetryAsync(
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync)
            : this(TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, generalAspect, null, beforeAsync, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, delay, asynchronous general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="delay"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        protected WithAspectAndRetryAsync(
            IRetryStrategy retryStrategy,
            int delay,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync)
            : this(TimeSpan.FromMilliseconds(delay), retryStrategy, null, generalAspectAsync, beforeAsync, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, 20ms delay, asynchronous general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        protected WithAspectAndRetryAsync(
            IRetryStrategy retryStrategy,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync)
            : this(TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, null, generalAspectAsync, beforeAsync, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, delay, general aspect, asynchronous general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="delay"></param>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        protected WithAspectAndRetryAsync(
            IRetryStrategy retryStrategy,
            int delay,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync)
            :this(TimeSpan.FromMilliseconds(delay), retryStrategy, generalAspect, generalAspectAsync, beforeAsync, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with retry strategy, 20ms delay, general aspect, asynchronous general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        protected WithAspectAndRetryAsync(
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync)
            : this(TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, generalAspect, generalAspectAsync, beforeAsync, null)
        {
        }

        /// <summary>
        /// Invoke logic with retry.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected new async Task<TOutput> InvokeAsync(TInput input)
        {
            int attempt = 0;
            int delayMiliseconds = _delayMiliseconds;

            while (true)
            {
                attempt++;

                try
                {
                    return await base.InvokeAsync(input).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    if (!_retryStrategy.Retry(CurrentOperationId, e, attempt, delayMiliseconds, x => { delayMiliseconds = x; }))
                    {
                        throw;
                    }
                }

                await Task.Delay(delayMiliseconds).ConfigureAwait(false);
            }
        }
    }
}
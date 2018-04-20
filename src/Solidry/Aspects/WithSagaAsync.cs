using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Contract.Factory;
using Solidry.Aspects.Internal;
using Solidry.Results;

namespace Solidry.Aspects
{
    /// <inheritdoc />
    /// <summary>
    /// With asynchronous saga.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class WithSagaAsync<TInput, TOutput> : WithAspectAndRetryAsync<TInput, TOutput>
    {
        private readonly IErrorHandlerStrategyAsync _errorHandlerStrategy;

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, delay, retry strategy, general aspect, asynchronous aspect, asynchronous before and after aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="delay"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        /// <param name="afterAsync"></param>
        protected WithSagaAsync(
            IErrorHandlerStrategyAsync errorHandlerStrategy,
            TimeSpan delay,
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync,
            IReadOnlyList<IAfterAspectAsync<TInput, TOutput>> afterAsync)
            : base(delay, retryStrategy, generalAspect, generalAspectAsync, beforeAsync, afterAsync)
        {
            _errorHandlerStrategy = errorHandlerStrategy;
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with factory.
        /// </summary>
        /// <param name="factory"></param>
        protected WithSagaAsync(ISagaAsyncFactory<TInput, TOutput> factory)
            : this(
                factory.ErrorHandlerStrategy,
                factory.Delay, factory.RetryStrategy,
                factory.GeneralAspect,
                factory.GeneralAspectAsync,
                factory.BeforeAsync,
                factory.AfterAsync)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, 20ms delay, retry strategy, general aspect, asynchronous aspect, asynchronous before and after aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        /// <param name="afterAsync"></param>
        protected WithSagaAsync(
            IErrorHandlerStrategyAsync errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync,
            IReadOnlyList<IAfterAspectAsync<TInput, TOutput>> afterAsync)
            : this(errorHandlerStrategy, TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, generalAspect, generalAspectAsync, beforeAsync, afterAsync)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, delay and general aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="delay"></param>
        /// <param name="generalAspect"></param>
        protected WithSagaAsync(
            IErrorHandlerStrategyAsync errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            int delay,
            IGeneralAspect generalAspect)
            : this(errorHandlerStrategy, TimeSpan.FromMilliseconds(delay), retryStrategy, generalAspect, null, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, 20ms delay and general aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        protected WithSagaAsync(
            IErrorHandlerStrategyAsync errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect)
            : this(errorHandlerStrategy, TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, generalAspect, null, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, delay, general aspect and asynchronous general aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="delay"></param>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        protected WithSagaAsync(
            IErrorHandlerStrategyAsync errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            int delay,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync)
            : this(errorHandlerStrategy, TimeSpan.FromMilliseconds(delay), retryStrategy, generalAspect, generalAspectAsync, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, 20ms delay, general aspect and asynchronous general aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        protected WithSagaAsync(
            IErrorHandlerStrategyAsync errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync)
            : this(errorHandlerStrategy, TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, generalAspect, generalAspectAsync, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, delay, general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="delay"></param>
        /// <param name="generalAspect"></param>
        /// <param name="beforeAsync"></param>
        protected WithSagaAsync(
            IErrorHandlerStrategyAsync errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            int delay,
            IGeneralAspect generalAspect,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync)
            : this(errorHandlerStrategy, TimeSpan.FromMilliseconds(delay), retryStrategy, generalAspect, null, beforeAsync, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, 20ms delay, general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="beforeAsync"></param>
        protected WithSagaAsync(
            IErrorHandlerStrategyAsync errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync)
            : this(errorHandlerStrategy, TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, generalAspect, null, beforeAsync, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, delay, asynchronous general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="delay"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        protected WithSagaAsync(
            IErrorHandlerStrategyAsync errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            int delay,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync)
            : this(errorHandlerStrategy, TimeSpan.FromMilliseconds(delay), retryStrategy, null, generalAspectAsync, beforeAsync, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, 20ms delay, asynchronous general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        protected WithSagaAsync(
            IErrorHandlerStrategyAsync errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync)
            : this(errorHandlerStrategy, TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, null, generalAspectAsync, beforeAsync, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, delay, general aspect, asynchronous general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="delay"></param>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        protected WithSagaAsync(
            IErrorHandlerStrategyAsync errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            int delay,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync)
            : this(errorHandlerStrategy, TimeSpan.FromMilliseconds(delay), retryStrategy, generalAspect, generalAspectAsync, beforeAsync, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, 20ms delay, general aspect, asynchronous general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        protected WithSagaAsync(
            IErrorHandlerStrategyAsync errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync)
            : this(errorHandlerStrategy, TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, generalAspect, generalAspectAsync, beforeAsync, null)
        {
        }

        /// <summary>
        /// Execute finally logic.
        /// Good place to dispose objects.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual Task FinallyAsync(TInput input)
        {
            (input as IDisposable)?.Dispose();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Execute logic with error handler.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected new async Task<Option<TOutput>> InvokeAsync(TInput input)
        {
            try
            {
                return Option<TOutput>.Create(await base.InvokeAsync(input).ConfigureAwait(false));
            }
            catch (Exception e)
            {
                if (!await _errorHandlerStrategy.TryHandleAsync(e, CurrentOperationId).ConfigureAwait(false))
                {
                    throw;
                }
            }
            finally
            {
                await FinallyAsync(input).ConfigureAwait(false);
            }

            return Option<TOutput>.Empty;
        }
    }
}
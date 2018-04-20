using System;
using System.Collections.Generic;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Contract.Factory;
using Solidry.Aspects.Internal;
using Solidry.Results;

namespace Solidry.Aspects
{
    /// <inheritdoc />
    /// <summary>
    /// With aspect and retry and error handler.
    /// </summary>
    public abstract class WithSaga<TInput, TOutput> : WithAspectAndRetry<TInput, TOutput>
    {
        private readonly IErrorHandlerStrategy _errorHandlerStrategy;

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, delay and general aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="delay"></param>
        /// <param name="generalAspect"></param>
        protected WithSaga(
            IErrorHandlerStrategy errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            int delay,
            IGeneralAspect generalAspect) :
            this(errorHandlerStrategy, TimeSpan.FromMilliseconds(delay), retryStrategy, generalAspect, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, 20ms delay and general aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        protected WithSaga(
            IErrorHandlerStrategy errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect) :
            this(errorHandlerStrategy, TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, generalAspect, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, delay and before aspect.
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="before"></param>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        protected WithSaga(
            IErrorHandlerStrategy errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            int delay,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before) :
            this(errorHandlerStrategy, TimeSpan.FromMilliseconds(delay), retryStrategy, null, before, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, 20ms delay and before aspect.
        /// </summary>
        /// <param name="before"></param>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        protected WithSaga(
            IErrorHandlerStrategy errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before) :
            this(errorHandlerStrategy, TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, null, before, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, delay, before and after aspect.
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        protected WithSaga(
            IErrorHandlerStrategy errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            int delay,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before,
            IReadOnlyList<IAfterAspect<TInput, TOutput>> after) :
            this(errorHandlerStrategy, TimeSpan.FromMilliseconds(delay), retryStrategy, null, before, after)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, retry strategy, 20ms delay, before and after aspect.
        /// </summary>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="retryStrategy"></param>
        protected WithSaga(
            IErrorHandlerStrategy errorHandlerStrategy,
            IRetryStrategy retryStrategy,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before,
            IReadOnlyList<IAfterAspect<TInput, TOutput>> after) :
            this(errorHandlerStrategy, TimeSpan.FromMilliseconds(Constant.DefaultDelay), retryStrategy, null, before, after)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, delay, retry strategy, general, before and after aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="delay"></param>
        /// <param name="retryStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        protected WithSaga(
            IErrorHandlerStrategy errorHandlerStrategy,
            TimeSpan delay,
            IRetryStrategy retryStrategy,
            IGeneralAspect generalAspect,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before,
            IReadOnlyList<IAfterAspect<TInput, TOutput>> after) :
            base(delay, retryStrategy, generalAspect, before, after)
        {
            _errorHandlerStrategy = errorHandlerStrategy;
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with factory.
        /// </summary>
        /// <param name="factory"></param>
        protected WithSaga(ISagaFactory<TInput, TOutput> factory) :
            this(
                factory.ErrorHandlerStrategy, factory.Delay, factory.RetryStrategy, factory.GeneralAspect, factory.Before, factory.After)
        {
        }

        /// <summary>
        /// Execute finally logic. It will execute always.
        /// Good spot do dispose objects.
        /// </summary>
        /// <param name="input"></param>
        protected virtual void Finally(TInput input)
        {
            (input as IDisposable)?.Dispose();
        }

        /// <summary>
        /// Execute logic with error handlers.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected new Option<TOutput> Invoke(TInput input)
        {
            try
            {
                return Option<TOutput>.Create(base.Invoke(input));
            }
            catch (Exception e)
            {
                if (!_errorHandlerStrategy.TryHandle(e, CurrentOperationId))
                {
                    throw;
                }
            }
            finally
            {
                Finally(input);
            }

            return Option<TOutput>.Empty;
        }
    }
}
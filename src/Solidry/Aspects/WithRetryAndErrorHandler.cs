using System;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Internal;
using Solidry.Results;

namespace Solidry.Aspects
{
    /// <inheritdoc />
    /// <summary>
    /// Aspect with retry and error handler.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class WithRetryAndErrorHandler<TInput, TOutput> : WithRetry<TInput, TOutput>
    {
        private readonly IErrorHandlerStrategy _errorHandlerStrategy;

        /// <inheritdoc />
        /// <summary>
        /// Create retry with 20ms and with error handler.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="errorHandlerStrategy"></param>
        protected WithRetryAndErrorHandler(IRetryStrategy retryStrategy, IErrorHandlerStrategy errorHandlerStrategy): this(retryStrategy, errorHandlerStrategy, TimeSpan.FromMilliseconds(Constant.DefaultDelay))
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create retry with error handler.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="delay"></param>
        protected WithRetryAndErrorHandler(IRetryStrategy retryStrategy, IErrorHandlerStrategy errorHandlerStrategy, TimeSpan delay): base(retryStrategy, delay)
        {
            _errorHandlerStrategy = errorHandlerStrategy;
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
                if (!_errorHandlerStrategy.TryHandle(e, OperationId))
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
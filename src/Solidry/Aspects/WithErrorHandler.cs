using System;
using Solidry.Aspects.Contract;
using Solidry.Results;

namespace Solidry.Aspects
{
    /// <summary>
    /// Aspect with error handler.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class WithErrorHandler<TInput, TOutput>
    {
        private readonly IErrorHandlerStrategy _errorHandlerStrategy;

        /// <inheritdoc />
        /// <summary>
        /// Create error handler.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        protected WithErrorHandler(IErrorHandlerStrategy errorHandlerStrategy)
        {
            _errorHandlerStrategy = errorHandlerStrategy;
        }

        /// <summary>
        /// Try execute logic.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract TOutput Try(TInput input);

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
        protected Option<TOutput> Invoke(TInput input)
        {
            try
            {
                return Option<TOutput>.Create(Try(input));
            }
            catch (Exception e)
            {
                if (!_errorHandlerStrategy.TryHandle(e, Guid.NewGuid()))
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
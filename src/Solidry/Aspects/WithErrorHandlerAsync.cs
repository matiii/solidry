using System;
using System.Threading.Tasks;
using Solidry.Aspects.Contract;
using Solidry.Results;

namespace Solidry.Aspects
{
    /// <summary>
    /// Aspect with error handler.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class WithErrorHandlerAsync<TInput, TOutput>
    {
        private readonly IErrorHandlerStrategyAsync _errorHandlerStrategy;

        /// <summary>
        /// Create with asynchronous error handler strategy.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        protected WithErrorHandlerAsync(IErrorHandlerStrategyAsync errorHandlerStrategy)
        {
            _errorHandlerStrategy = errorHandlerStrategy;
        }

        /// <summary>
        /// Try execute logic.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract Task<TOutput> TryAsync(TInput input);

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
        protected async Task<Option<TOutput>> InvokeAsync(TInput input)
        {
            try
            {
                return Option<TOutput>.Create(await TryAsync(input).ConfigureAwait(false));
            }
            catch (Exception e)
            {
                if (!await _errorHandlerStrategy.TryHandleAsync(e, Guid.NewGuid()).ConfigureAwait(false))
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
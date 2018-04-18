using System;
using System.Collections.Generic;
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
    public abstract class WithAspectAndErrorHandlerAsync<TInput, TOutput> : WithAspectAsync<TInput, TOutput>
    {
        private readonly IErrorHandlerStartegyAsync _errorHandlerStrategy;

        /// <summary>
        /// Create with error handler strategy, general aspect, asynchronous general aspect, asynchronous before and after aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        /// <param name="afterAsync"></param>
        protected WithAspectAndErrorHandlerAsync(
            IErrorHandlerStartegyAsync errorHandlerStrategy,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync,
            IReadOnlyList<IAfterAspectAsync<TInput, TOutput>> afterAsync)
            :base(generalAspect, generalAspectAsync, beforeAsync, afterAsync)
        {
            _errorHandlerStrategy = errorHandlerStrategy;
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy and general aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="generalAspect"></param>
        protected WithAspectAndErrorHandlerAsync(
            IErrorHandlerStartegyAsync errorHandlerStrategy,
            IGeneralAspect generalAspect) : 
            this(errorHandlerStrategy, generalAspect, null, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, general aspect and asynchronous general aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        protected WithAspectAndErrorHandlerAsync(
            IErrorHandlerStartegyAsync errorHandlerStrategy,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync) :
            this(errorHandlerStrategy, generalAspect, generalAspectAsync, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="beforeAsync"></param>
        protected WithAspectAndErrorHandlerAsync(
            IErrorHandlerStartegyAsync errorHandlerStrategy,
            IGeneralAspect generalAspect,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync) : 
            this(errorHandlerStrategy, generalAspect, null, beforeAsync, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, asynchronous general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        protected WithAspectAndErrorHandlerAsync(
            IErrorHandlerStartegyAsync errorHandlerStrategy,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync) :
            this(errorHandlerStrategy, null, generalAspectAsync, beforeAsync)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, general aspect, asynchronous general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        protected WithAspectAndErrorHandlerAsync(
            IErrorHandlerStartegyAsync errorHandlerStrategy,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync)
            : this(errorHandlerStrategy, generalAspect, generalAspectAsync, beforeAsync, null)
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
            catch (Exception ex)
            {
                if (!await _errorHandlerStrategy.TryHandleAsync(ex, CurrentOperationId).ConfigureAwait(false))
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
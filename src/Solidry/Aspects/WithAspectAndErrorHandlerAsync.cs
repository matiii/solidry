using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solidry.Aspects.Contract;
using Solidry.Results;

namespace Solidry.Aspects
{
    public abstract class WithAspectAndErrorHandlerAsync<TInput, TOutput> : WithAspectAsync<TInput, TOutput>
    {
        private readonly IErrorHandlerStartegyAsync _errorHandlerStrategy;

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

        protected WithAspectAndErrorHandlerAsync(
            IErrorHandlerStartegyAsync errorHandlerStrategy,
            IGeneralAspect generalAspect) : 
            this(errorHandlerStrategy, generalAspect, null, null, null)
        {
        }

        protected WithAspectAndErrorHandlerAsync(
            IErrorHandlerStartegyAsync errorHandlerStrategy,
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync) :
            this(errorHandlerStrategy, generalAspect, generalAspectAsync, null, null)
        {
        }

        protected WithAspectAndErrorHandlerAsync(
            IErrorHandlerStartegyAsync errorHandlerStrategy,
            IGeneralAspect generalAspect,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync) : 
            this(errorHandlerStrategy, generalAspect, null, beforeAsync, null)
        {
        }

        protected WithAspectAndErrorHandlerAsync(
            IErrorHandlerStartegyAsync errorHandlerStrategy,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync) :
            this(errorHandlerStrategy, null, generalAspectAsync, beforeAsync)
        {
        }

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
                if (await _errorHandlerStrategy.TryHandleAsync(ex, CurrentOperationId).ConfigureAwait(false))
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
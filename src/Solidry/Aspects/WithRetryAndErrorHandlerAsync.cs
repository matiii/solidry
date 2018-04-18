﻿using System;
using System.Threading.Tasks;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Internal;
using Solidry.Results;

namespace Solidry.Aspects
{
    /// <inheritdoc />
    /// <summary>
    /// Aspect with retry and error handling.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class WithRetryAndErrorHandlerAsync<TInput, TOutput> : WithRetryAsync<TInput, TOutput>
    {
        private readonly IErrorHandlerStartegyAsync _errorHandlerStartegy;

        /// <inheritdoc />
        /// <summary>
        /// Create retry aspect with error handler.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="errorHandlerStartegy"></param>
        /// <param name="delay"></param>
        protected WithRetryAndErrorHandlerAsync(IRetryStrategy retryStrategy, IErrorHandlerStartegyAsync errorHandlerStartegy, TimeSpan delay): base(retryStrategy, delay)
        {
            _errorHandlerStartegy = errorHandlerStartegy;
        }

        /// <inheritdoc />
        /// <summary>
        /// Create retry aspect with 20ms delay and error handler.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="errorHandlerStartegy"></param>
        protected WithRetryAndErrorHandlerAsync(IRetryStrategy retryStrategy, IErrorHandlerStartegyAsync errorHandlerStartegy): this(retryStrategy, errorHandlerStartegy, TimeSpan.FromMilliseconds(Constant.DefaultDelay))
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
                if (!await _errorHandlerStartegy.TryHandleAsync(e, OperationId).ConfigureAwait(false))
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Solidry.Aspects.Contract;
using Solidry.Results;

namespace Solidry.Aspects
{
    /// <summary>
    /// Asynchronous aspect.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class WithAspectAsync<TInput, TOutput>
    {
        private readonly IGeneralAspect _generalAspect;
        private readonly IGeneralAspectAsync _generalAspectAsync;

        private readonly IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> _beforeAsync;
        private readonly IReadOnlyList<IAfterAspectAsync<TInput, TOutput>> _afterAsync;

        /// <summary>
        /// Create with general aspect, asynchronous general aspect, asynchronous before and after aspect.
        /// </summary>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        /// <param name="afterAsync"></param>
        protected WithAspectAsync(
            IGeneralAspect generalAspect,
            IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync,
            IReadOnlyList<IAfterAspectAsync<TInput, TOutput>> afterAsync)
        {
            _generalAspect = generalAspect;
            _generalAspectAsync = generalAspectAsync;
            _beforeAsync = beforeAsync;
            _afterAsync = afterAsync;
        }

        /// <summary>
        /// Create with general aspect.
        /// </summary>
        /// <param name="generalAspect"></param>
        protected WithAspectAsync(IGeneralAspect generalAspect) : this(generalAspect, null, null)
        {
        }

        /// <summary>
        /// Create with general aspect and asynchronous general aspect.
        /// </summary>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        protected WithAspectAsync(IGeneralAspect generalAspect, IGeneralAspectAsync generalAspectAsync) :
            this(generalAspect, generalAspectAsync, null)
        {
        }

        /// <summary>
        /// Create with general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="generalAspect"></param>
        /// <param name="beforeAsync"></param>
        protected WithAspectAsync(IGeneralAspect generalAspect,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync) : this(generalAspect, null, beforeAsync)
        {
        }

        /// <summary>
        /// Create with asynchronous general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        protected WithAspectAsync(IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync) : this(null, generalAspectAsync,
            beforeAsync)
        {
        }

        /// <summary>
        /// Create with general aspect, asynchronous general aspect and asynchronous before aspect.
        /// </summary>
        /// <param name="generalAspect"></param>
        /// <param name="generalAspectAsync"></param>
        /// <param name="beforeAsync"></param>
        protected WithAspectAsync(IGeneralAspect generalAspect, IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync)
            : this(generalAspect, generalAspectAsync, beforeAsync, null)
        {
        }

        /// <summary>
        /// Execute asynchronous logic.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract Task<TOutput> ExecuteAsync(TInput input);

        /// <summary>
        /// Get current operation id
        /// </summary>
        protected Guid CurrentOperationId { get; private set; }

        /// <summary>
        /// Execute logic with aspects.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected async Task<TOutput> InvokeAsync(TInput input)
        {
            CurrentOperationId = Guid.NewGuid();
            var stopWatch = Stopwatch.StartNew();

            Option<TOutput> result = Option<TOutput>.Empty;

            if (_generalAspect != null)
            {
                result = _generalAspect.Before<TInput, TOutput>(input, CurrentOperationId);

                if (result.HasValue)
                {
                    return result.Value;
                }
            }

            if (_generalAspectAsync != null)
            {
                result = await _generalAspectAsync.BeforeAsync<TInput, TOutput>(input, CurrentOperationId)
                    .ConfigureAwait(false);

                if (result.HasValue)
                {
                    return result.Value;
                }
            }

            if (_beforeAsync != null)
            {
                for (int i = 0; i < _beforeAsync.Count; i++)
                {
                    result = await _beforeAsync[i].BeforeAsync(input, CurrentOperationId).ConfigureAwait(false);

                    if (result.HasValue)
                    {
                        return result.Value;
                    }
                }
            }

            TOutput output = await ExecuteAsync(input);

            stopWatch.Stop();

            if (_afterAsync != null)
            {
                for (int i = 0; i < _afterAsync.Count; i++)
                {
                    await _afterAsync[i].AfterAsync(input, output, CurrentOperationId, stopWatch.Elapsed)
                        .ConfigureAwait(false);
                }
            }

            if (_generalAspectAsync != null)
            {
                await _generalAspectAsync.AfterAsync(input, output, CurrentOperationId, stopWatch.Elapsed)
                    .ConfigureAwait(false);
            }

            _generalAspect?.After(input, output, CurrentOperationId, stopWatch.Elapsed);

            return output;
        }
    }
}
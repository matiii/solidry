using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Solidry.Aspects.Contract;
using Solidry.Results;

namespace Solidry.Aspects
{
    public abstract class WithAspectAsync<TInput, TOutput>
    {
        private readonly IGeneralAspect _generalAspect;
        private readonly IGeneralAspectAsync _generalAspectAsync;

        private readonly IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> _beforeAsync;
        private readonly IReadOnlyList<IAfterAspectAsync<TInput, TOutput>> _afterAsync;

        protected WithAspectAsync(IGeneralAspect generalAspect, IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync,
            IReadOnlyList<IAfterAspectAsync<TInput, TOutput>> afterAsync)
        {
            _generalAspect = generalAspect;
            _generalAspectAsync = generalAspectAsync;
            _beforeAsync = beforeAsync;
            _afterAsync = afterAsync;
        }

        protected WithAspectAsync(IGeneralAspect generalAspect) : this(generalAspect, null, null)
        {
        }

        protected WithAspectAsync(IGeneralAspect generalAspect, IGeneralAspectAsync generalAspectAsync) :
            this(generalAspect, generalAspectAsync, null)
        {
        }

        protected WithAspectAsync(IGeneralAspect generalAspect,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync) : this(generalAspect, null, beforeAsync)
        {
        }

        protected WithAspectAsync(IGeneralAspectAsync generalAspectAsync,
            IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> beforeAsync) : this(null, generalAspectAsync,
            beforeAsync)
        {
        }

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
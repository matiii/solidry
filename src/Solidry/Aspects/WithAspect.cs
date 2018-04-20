using System;
using System.Collections.Generic;
using System.Diagnostics;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Contract.Factory;
using Solidry.Extensions;
using Solidry.Results;

namespace Solidry.Aspects
{
    /// <summary>
    /// With aspect.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class WithAspect<TInput, TOutput>
    {
        private readonly IReadOnlyList<IBeforeAspect<TInput, TOutput>> _before;
        private readonly IReadOnlyList<IAfterAspect<TInput, TOutput>> _after;
        private readonly IGeneralAspect _generalAspect;

        /// <inheritdoc />
        /// <summary>
        /// Create with general aspect.
        /// </summary>
        /// <param name="generalAspect"></param>
        protected WithAspect(IGeneralAspect generalAspect) : this(generalAspect, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with before aspect.
        /// </summary>
        /// <param name="before"></param>
        protected WithAspect(IReadOnlyList<IBeforeAspect<TInput, TOutput>> before) : this(null, before, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with before and after aspect.
        /// </summary>
        /// <param name="before"></param>
        /// <param name="after"></param>
        protected WithAspect(IReadOnlyList<IBeforeAspect<TInput, TOutput>> before,
            IReadOnlyList<IAfterAspect<TInput, TOutput>> after) : this(null, before, after)
        {
        }

        /// <summary>
        /// Create with aspect factory.
        /// </summary>
        /// <param name="factory"></param>
        protected WithAspect(IAspectFactory<TInput, TOutput> factory)
            : this(factory.GeneralAspect, factory.Before, factory.After)
        {
        }

        /// <summary>
        /// Create with general, before and after aspect.
        /// </summary>
        /// <param name="generalAspect"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        protected WithAspect(IGeneralAspect generalAspect, IReadOnlyList<IBeforeAspect<TInput, TOutput>> before,
            IReadOnlyList<IAfterAspect<TInput, TOutput>> after)
        {
            _generalAspect = generalAspect;
            _before = before;
            _after = after;
        }

        /// <summary>
        /// Implementation of logic.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract TOutput Execute(TInput input);

        /// <summary>
        /// Get current operation id
        /// </summary>
        protected Guid CurrentOperationId { get; private set; }

        /// <summary>
        /// Invoke logic with aspects.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected TOutput Invoke(TInput input)
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

            if (_before != null)
            {
                for (int i = 0; i < _before.Count; i++)
                {
                    result = _before[i].Before(input, CurrentOperationId);

                    if (result.HasValue)
                    {
                        return result.Value;
                    }
                }
            }

            TOutput output = Execute(input);

            stopWatch.Stop();

            _after?.Each(x => x.After(input, output, CurrentOperationId, stopWatch.Elapsed));

            _generalAspect?.After(input, output, CurrentOperationId, stopWatch.Elapsed);

            return output;
        }
    }
}
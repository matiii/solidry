using System;
using System.Collections.Generic;
using Solidry.Extensions;
using Solidry.Results;

namespace Solidry.Aspects
{
    public abstract class WithAspect<TInput, TOutput>
    {
        private readonly List<Func<TInput, Option<TOutput>>> _before = new List<Func<TInput, Option<TOutput>>>();
        private readonly List<Action<TInput, TOutput>> _after = new List<Action<TInput, TOutput>>();

        /// <summary>
        /// Implementation of logic.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract TOutput Execute(TInput input);

        /// <summary>
        /// Register aspects.
        /// </summary>
        protected abstract void RegisterAspects();

        /// <summary>
        /// Invoke logic with aspects.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected TOutput Invoke(TInput input)
        {
            for (int i = 0; i < _before.Count; i++)
            {
                Option<TOutput> result = _before[i](input);

                if (result.HasValue)
                {
                    return result.Value;
                }
            }

            TOutput output = Execute(input);

            _after.Each(x => x(input, output));

            return output;
        }

        /// <summary>
        /// Add before aspect.
        /// </summary>
        /// <param name="before"></param>
        protected void AddBefore(Func<TInput, Option<TOutput>> before)
        {
            _before.Add(before);
        }

        /// <summary>
        /// Add after aspect.
        /// </summary>
        /// <param name="after"></param>
        protected void AddAfter(Action<TInput, TOutput> after)
        {
            _after.Add(after);
        }
    }
}
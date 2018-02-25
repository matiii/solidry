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

        protected abstract TOutput Execute(TInput input);

        protected abstract void RegisterAspects();

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
    }
}
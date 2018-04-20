using System;
using System.Collections.Generic;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Contract.Factory;

namespace Solidry.Aspects.Factory
{
    public class AspectAndRetryFactoryBase<TInput, TOutput> : IAspectAndRetryFactory<TInput, TOutput>
    {
        public TimeSpan Delay { get; set; }

        public IRetryStrategy RetryStrategy { get; set; }

        public IGeneralAspect GeneralAspect { get; set; }

        public IReadOnlyList<IBeforeAspect<TInput, TOutput>> Before { get; set; }

        public IReadOnlyList<IAfterAspect<TInput, TOutput>> After { get; set; }
    }
}
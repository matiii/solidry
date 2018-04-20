using System;
using System.Collections.Generic;

namespace Solidry.Aspects.Contract.Factory
{
    public interface IAspectAndRetryFactory<TInput, TOutput>
    {
        TimeSpan Delay { get; }

        IRetryStrategy RetryStrategy { get; }

        IGeneralAspect GeneralAspect { get; }

        IReadOnlyList<IBeforeAspect<TInput, TOutput>> Before { get; }

        IReadOnlyList<IAfterAspect<TInput, TOutput>> After { get; }
    }
}
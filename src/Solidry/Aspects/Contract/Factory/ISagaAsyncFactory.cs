using System;
using System.Collections.Generic;

namespace Solidry.Aspects.Contract.Factory
{
    public interface ISagaAsyncFactory<TInput, TOutput>
    {
        IErrorHandlerStrategyAsync ErrorHandlerStrategy { get; }

        TimeSpan Delay { get; }

        IRetryStrategy RetryStrategy { get; }

        IGeneralAspect GeneralAspect { get; }

        IGeneralAspectAsync GeneralAspectAsync { get; }

        IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> BeforeAsync { get; }

        IReadOnlyList<IAfterAspectAsync<TInput, TOutput>> AfterAsync { get; }
    }
}
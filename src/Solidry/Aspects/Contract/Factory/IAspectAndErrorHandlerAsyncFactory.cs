using System.Collections.Generic;

namespace Solidry.Aspects.Contract.Factory
{
    public interface IAspectAndErrorHandlerAsyncFactory<TInput, TOutput>
    {
        IErrorHandlerStrategyAsync ErrorHandlerStrategy { get; }

        IGeneralAspect GeneralAspect { get; }

        IGeneralAspectAsync GeneralAspectAsync { get; }

        IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> BeforeAsync { get; }

        IReadOnlyList<IAfterAspectAsync<TInput, TOutput>> AfterAsync { get; }
    }
}
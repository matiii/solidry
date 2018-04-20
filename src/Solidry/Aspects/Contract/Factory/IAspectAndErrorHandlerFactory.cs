using System.Collections.Generic;

namespace Solidry.Aspects.Contract.Factory
{
    public interface IAspectAndErrorHandlerFactory<TInput, TOutput>
    {
        IErrorHandlerStrategy ErrorHandlerStrategy { get; }

        IGeneralAspect GeneralAspect { get; }

        IReadOnlyList<IBeforeAspect<TInput, TOutput>> Before { get; }

        IReadOnlyList<IAfterAspect<TInput, TOutput>> After { get; }
    }
}
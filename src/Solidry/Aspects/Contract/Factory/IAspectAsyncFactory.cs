using System.Collections.Generic;

namespace Solidry.Aspects.Contract.Factory
{
    public interface IAspectAsyncFactory<TInput, TOutput>
    {
        IGeneralAspect GeneralAspect { get; }

        IGeneralAspectAsync GeneralAspectAsync { get; }

        IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> BeforeAsync { get; }

        IReadOnlyList<IAfterAspectAsync<TInput, TOutput>> AfterAsync { get; }
    }
}
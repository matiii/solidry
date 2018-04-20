using System.Collections.Generic;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Contract.Factory;

namespace Solidry.Aspects.Factory
{
    public class AspectFactoryBase<TInput, TOutput> : IAspectFactory<TInput, TOutput>
    {
        public IGeneralAspect GeneralAspect { get; set; }

        public IReadOnlyList<IBeforeAspect<TInput, TOutput>> Before { get; set; }

        public IReadOnlyList<IAfterAspect<TInput, TOutput>> After { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Contract.Factory;

namespace Solidry.Aspects.Factory
{
    public class AspectAndErrorHandlerFactoryBase<TInput, TOutput> : IAspectAndErrorHandlerFactory<TInput, TOutput>
    {
        public AspectAndErrorHandlerFactoryBase(IErrorHandlerStrategy errorHandlerStrategy)
        {
            ErrorHandlerStrategy = errorHandlerStrategy ?? throw new ArgumentNullException("Error handler strategy cannot be null.");
        }

        public IErrorHandlerStrategy ErrorHandlerStrategy { get; }

        public IGeneralAspect GeneralAspect { get; set; }

        public IReadOnlyList<IBeforeAspect<TInput, TOutput>> Before { get; set; }

        public IReadOnlyList<IAfterAspect<TInput, TOutput>> After { get; set; }
    }
}
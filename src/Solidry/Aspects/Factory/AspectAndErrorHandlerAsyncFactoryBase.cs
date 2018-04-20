using System;
using System.Collections.Generic;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Contract.Factory;

namespace Solidry.Aspects.Factory
{
    public class AspectAndErrorHandlerAsyncFactoryBase<TInput, TOutput> : IAspectAndErrorHandlerAsyncFactory<TInput, TOutput>
    {
        public AspectAndErrorHandlerAsyncFactoryBase(IErrorHandlerStrategyAsync errorHandlerStrategy)
        {
            ErrorHandlerStrategy = errorHandlerStrategy ?? throw new ArgumentNullException("Error handler strategy cannot be null.");
        }

        public IErrorHandlerStrategyAsync ErrorHandlerStrategy { get; }

        public IGeneralAspect GeneralAspect { get; set; }

        public IGeneralAspectAsync GeneralAspectAsync { get; set; }

        public IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> BeforeAsync { get; set; }

        public IReadOnlyList<IAfterAspectAsync<TInput, TOutput>> AfterAsync { get; set; }
    }
}
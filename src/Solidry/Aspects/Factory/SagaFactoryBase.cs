using System;
using System.Collections.Generic;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Contract.Factory;

namespace Solidry.Aspects.Factory
{
    public class SagaFactoryBase<TInput, TOutput> : ISagaFactory<TInput, TOutput>
    {
        public SagaFactoryBase(IErrorHandlerStrategy errorHandlerStrategy, IRetryStrategy retryStrategy)
        {
            ErrorHandlerStrategy = errorHandlerStrategy ?? throw new ArgumentNullException("Error handler strategy cannot be null.");
            RetryStrategy = retryStrategy ?? throw new ArgumentNullException("Retry strategy cannot be null.");
        }

        public IErrorHandlerStrategy ErrorHandlerStrategy { get; }

        public TimeSpan Delay { get; set; }

        public IRetryStrategy RetryStrategy { get; }

        public IGeneralAspect GeneralAspect { get; set; }

        public IReadOnlyList<IBeforeAspect<TInput, TOutput>> Before { get; set; }

        public IReadOnlyList<IAfterAspect<TInput, TOutput>> After { get; set; }
    }
}
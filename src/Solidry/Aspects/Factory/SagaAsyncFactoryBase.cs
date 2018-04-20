using System;
using System.Collections.Generic;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Contract.Factory;

namespace Solidry.Aspects.Factory
{
    public class SagaAsyncFactoryBase<TInput, TOutput> : ISagaAsyncFactory<TInput, TOutput>
    {
        public SagaAsyncFactoryBase(IErrorHandlerStrategyAsync errorHandlerStrategy, IRetryStrategy retryStrategy)
        {
            ErrorHandlerStrategy = errorHandlerStrategy ?? throw new ArgumentNullException("Error handler strategy cannot be null.");
            RetryStrategy = retryStrategy ?? throw new ArgumentNullException("Retry strategy cannot be null.");
        }

        public IErrorHandlerStrategyAsync ErrorHandlerStrategy { get; }

        public IRetryStrategy RetryStrategy { get; }

        public TimeSpan Delay { get; set; }

        public IGeneralAspect GeneralAspect { get; set; }

        public IGeneralAspectAsync GeneralAspectAsync { get; set; }

        public IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> BeforeAsync { get; set; }

        public IReadOnlyList<IAfterAspectAsync<TInput, TOutput>> AfterAsync { get; set; }
    }
}
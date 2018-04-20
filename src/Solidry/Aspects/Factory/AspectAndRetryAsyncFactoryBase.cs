using System;
using System.Collections.Generic;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Contract.Factory;

namespace Solidry.Aspects.Factory
{
    public class AspectAndRetryAsyncFactoryBase<TInput, TOutput> : IAspectAndRetryAsyncFactory<TInput, TOutput>
    {
        public TimeSpan Delay { get; set; }

        public IRetryStrategy RetryStrategy { get; set; }

        public IGeneralAspect GeneralAspect { get; set; }

        public IGeneralAspectAsync GeneralAspectAsync { get; set; }

        public IReadOnlyList<IBeforeAspectAsync<TInput, TOutput>> BeforeAsync { get; set; }

        public IReadOnlyList<IAfterAspectAsync<TInput, TOutput>> AfterAsync { get; set; }
    }
}
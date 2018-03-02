using System;
using Solidry.Results;

namespace Solidry.Aspects.Saga
{
    public class SagaEvent<TInput, TOutput>
    {
        public Option<TInput> Input { get; }

        public Option<TOutput> Output { get; }

        public Option<Exception> Exception { get; }

        public SagaEventType Type { get; }

        internal SagaEvent(TInput input)
        {
            Type = SagaEventType.StartProcessing;
            Input = Option<TInput>.Create(input);
            Output = Option<TOutput>.Empty;
            Exception = Option<Exception>.Empty;
        }

        internal SagaEvent(TOutput output)
        {
            Type = SagaEventType.EndProcessing;
            Input = Option<TInput>.Empty;
            Output = Option<TOutput>.Create(output);
            Exception = Option<Exception>.Empty;
        }

        internal SagaEvent(Exception exception)
        {
            Type = SagaEventType.Error;
            Input = Option<TInput>.Empty;
            Output = Option<TOutput>.Empty;
            Exception = Option<Exception>.Create(exception);
        }
    }
}
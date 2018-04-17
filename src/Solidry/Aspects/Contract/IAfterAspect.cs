using System;

namespace Solidry.Aspects.Contract
{
    public interface IAfterAspect<TInput, TOutput>
    {
        void After(TInput input, TOutput output, Guid operationId, TimeSpan operationElapsed);
    }
}
using System;
using Solidry.Results;

namespace Solidry.Aspects.Contract
{
    public interface IGeneralAspect
    {
        Option<TOutput> Before<TInput, TOutput>(TInput input, Guid operationId);

        void After<TInput, TOutput>(TInput input, TOutput output, Guid operationId, TimeSpan operationElapsed);
    }
}
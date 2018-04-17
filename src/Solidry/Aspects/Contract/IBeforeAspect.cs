using System;
using Solidry.Results;

namespace Solidry.Aspects.Contract
{
    public interface IBeforeAspect<in TInput, TOutput>
    {
        Option<TOutput> Before(TInput input, Guid operationId);
    }
}
using System;
using System.Threading.Tasks;

namespace Solidry.Aspects.Contract
{
    public interface IAfterAspectAsync<TInput, TOutput>
    {
        Task AfterAsync(TInput input, TOutput output, Guid operationId, TimeSpan operationElapsed);
    }
}
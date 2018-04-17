using System;
using System.Threading.Tasks;
using Solidry.Results;

namespace Solidry.Aspects.Contract
{
    public interface IBeforeAspectAsync<in TInput, TOutput>
    {
        Task<Option<TOutput>> BeforeAsync(TInput input, Guid operationId);
    }
}
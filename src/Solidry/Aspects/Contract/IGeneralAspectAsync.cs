using System;
using System.Threading.Tasks;
using Solidry.Results;

namespace Solidry.Aspects.Contract
{
    public interface IGeneralAspectAsync
    {
        Task<Option<TOutput>> BeforeAsync<TInput, TOutput>(TInput input, Guid operationId);

        Task AfterAsync<TInput, TOutput>(TInput input, TOutput output, Guid operationId, TimeSpan operationElapsed);
    }
}
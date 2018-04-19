using System;
using System.Threading.Tasks;

namespace Solidry.Aspects.Contract
{
    public interface IErrorHandlerStrategyAsync
    {
        Task<bool> TryHandleAsync(Exception exception, Guid operationId);
    }
}
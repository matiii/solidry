using System;
using System.Threading.Tasks;

namespace Solidry.Aspects.Contract
{
    public interface IErrorHandlerStartegyAsync
    {
        Task<bool> TryHandleAsync(Exception exception, Guid operationId);
    }
}
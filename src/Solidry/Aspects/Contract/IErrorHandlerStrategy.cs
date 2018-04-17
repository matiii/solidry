using System;

namespace Solidry.Aspects.Contract
{
    public interface IErrorHandlerStrategy
    {
        bool TryHandle(Exception exception, Guid operationId);
    }
}
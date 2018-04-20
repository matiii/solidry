using System;
using Solidry.Aspects.Contract;

namespace Solidry.Examples.Aspects.WithErrorHandler
{
    public class ExceptionHandler: IErrorHandlerStrategy
    {
        public bool TryHandle(Exception exception, Guid operationId)
        {
            if (exception is DivideByZeroException)
            {
                Console.WriteLine("It must happend.");

                return true;
            }

            if (exception.Message.Equals("Input should be more than 0"))
            {
                Console.WriteLine("You set input to 0, should be greater than 0.");

                return true;
            }

            return false;
        }
    }
}
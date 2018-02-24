using System;
using Solidry.Aspects;
using Solidry.Results;

namespace Solidry.Examples.Aspects.WithErrorHandler
{
    public class DivideByZero: WithErrorHandler<int,int>
    {
        protected override int Try(int input)
        {
            if (input == 0)
            {
                throw new Exception("Input should be more than 0");
            }

            if (input == 1)
            {
                throw new Exception("Not handled exception.");
            }

            return input / 0;
        }

        protected override void RegisterErrorhandlers()
        {
            AddErrorHandler<Exception>(e =>
            {
                if (e.Message.Equals("Input should be more than 0"))
                {
                    Console.WriteLine("You set input to 0, should be greater than 0.");

                    return true;
                }

                return false;
            });

            AddErrorHandler<DivideByZeroException>(e =>
            {
                Console.WriteLine("It must happend.");

                return true;
            });
        }

        public Option<int> Execute(int input)
        {
            return Invoke(input);
        }
    }
}
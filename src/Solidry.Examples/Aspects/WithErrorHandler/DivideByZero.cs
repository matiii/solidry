using System;
using Solidry.Aspects;
using Solidry.Aspects.Contract;
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
        

        public Option<int> Execute(int input)
        {
            return Invoke(input);
        }

        public DivideByZero() : base(new ExceptionHandler())
        {
        }
    }
}
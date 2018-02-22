using Solidry.Aspects;
using Solidry.Extensions;

namespace Solidry.Examples.Aspects.WithProcessor
{
    public class Fibonacci : WithProcessor<int, int>
    {
        public static int Fib(int n)
        {
            if (n == 0)
            {
                return 0;
            }

            if (n == 1 || n == 2)
            {
                return 1;
            }

            return Fib(n - 1) + Fib(n - 2);
        }

        protected override bool FinishLoop(int context)
        {
            return Accumulator.Count == context + 1;
        }

        protected override int Process(int context)
        {
            if (Accumulator.IsEmpty())
            {
                return 0;
            }
            
            if (Accumulator.Count < 2)
            {
                return 1;
            }

            return Accumulator.BeforeLast() + Accumulator.Last();
        }
    }
}
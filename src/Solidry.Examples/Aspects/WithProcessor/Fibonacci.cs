using System.Linq;
using Solidry.Aspects;
using Solidry.Extensions;

namespace Solidry.Examples.Aspects.WithProcessor
{
    public class Fibonacci : WithProcessor<int, int>
    {
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

        public int Return(int i)
        {
            return Invoke(i).Value;
        }

        public int[] GetRow(int i)
        {
            Invoke(i);

            return Accumulator.ToArray();
        }
    }
}
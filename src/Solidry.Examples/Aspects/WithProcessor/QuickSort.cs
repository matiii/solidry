using System;
using System.Linq;
using Solidry.Aspects;
using Solidry.Extensions;

namespace Solidry.Examples.Aspects.WithProcessor
{
    public class QuickSort: WithProcessor<int[], int[]>
    {
        private int _step = 1;
//        let rec quicksort2 = function
//        | [] -> []                         
//        | first::rest -> 
//        let smaller, larger = List.partition((>=) first) rest
//            List.concat [quicksort2 smaller;[first]; quicksort2 larger]
        protected override bool FinishLoop(int[] context)
        {
            return _step == 3;
        }

        protected override int[] Process(int[] context)
        {
            if (context.IsEmpty() || context.Length == 1)
            {
                _step = 3;
                return context;
            }

            if (Accumulator.IsEmpty())
            {
                return context;
            }

            var data = Accumulator.Last();

            if (data.IsEmpty())
            {
                _step++;
            }

            var first = data.First();
            var rest = data.Skip(1).ToArray();

            if (_step == 1)
            {
                return rest.Where(x => x <= first).ToArray();
            }

            if (Accumulator.IsEmpty())
            {
            }

            


            var larger = rest.Where(x => x > first).ToArray();
        }
    }
}